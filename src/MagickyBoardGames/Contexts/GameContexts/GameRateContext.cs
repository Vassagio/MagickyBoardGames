using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameRateContext : IGameRateContext {
        private readonly IGameRepository _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;
        private readonly IGamePlayerRatingRepository _gamePlayerRatingRepository;
        private readonly IBuilder<GamePlayerRating, PlayerRatingViewModel> _playerRatingBuilder;

        public GameRateContext(IGameRepository gameRepository,
                               IBuilder<Game, GameViewModel> gameBuilder,
                               IRatingRepository ratingRepository,
                               IBuilder<Rating, RatingViewModel> ratingBuilder,
                               IGamePlayerRatingRepository gamePlayerRatingRepository,
                               IBuilder<GamePlayerRating, PlayerRatingViewModel> playerRatingBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
            _gamePlayerRatingRepository = gamePlayerRatingRepository;
            _playerRatingBuilder = playerRatingBuilder;
        }

        public async Task<GameRateViewModel> BuildViewModel(int gameId, string playerId) {
            var viewModel = await BuildViewModel();
            var game = await _gameRepository.GetBy(gameId);
            if (game != null)
                viewModel.Game = _gameBuilder.Build(game);
            var gamePlayerRating = await _gamePlayerRatingRepository.GetBy(gameId, playerId);
            if (gamePlayerRating != null)
                viewModel.RatingId = gamePlayerRating.RatingId;
            viewModel.UserId = playerId;
            viewModel.PlayerRatings = GetPlayerRatings(game) ?? new List<PlayerRatingViewModel>();
            return viewModel;
        }

        public async Task<GameRateViewModel> BuildViewModel() {
            return new GameRateViewModel {
                AvailableRatings = await BuildRatingViewModels()
            };
        }

        private IEnumerable<PlayerRatingViewModel> GetPlayerRatings(Game game) {
            return game.GamePlayerRatings?.Select(gamePlayerRating => _playerRatingBuilder.Build(gamePlayerRating)).ToList();
        }

        public async Task Save(GameRateViewModel viewModel) {
            if (viewModel.Game?.Id != null)
                await _gamePlayerRatingRepository.Save(viewModel.Game.Id.Value, viewModel.UserId, viewModel.RatingId);
        }

        private async Task<IEnumerable<RatingViewModel>> BuildRatingViewModels() {
            var ratings = await _ratingRepository.GetAll();
            return ratings.Select(r => _ratingBuilder.Build(r)).ToList();
        }
    }
}
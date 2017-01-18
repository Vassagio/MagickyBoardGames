using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public class GameRatingBuilder : IBuilder<GamePlayerRating, GameRatingViewModel> {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;
        private readonly IGameRepository _gameRepository;
        private string _gameName;
        private RatingViewModel _rating;

        public GameRatingBuilder(IRatingRepository ratingRepository, IBuilder<Rating, RatingViewModel> ratingBuilder, IGameRepository gameRepository) {
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
            _gameRepository = gameRepository;
        }

        public GameRatingViewModel ToViewModel() {
            return new GameRatingViewModel {
                GameName = _gameName,
                Rating = _rating
            };
        }

        public GameRatingViewModel Build(GamePlayerRating entity) {
            var game = _gameRepository.GetBy(entity.GameId).Result;
            var rating = _ratingRepository.GetBy(entity.RatingId).Result;
            if (game == null || rating == null)
                return new GameRatingViewModel();
            _gameName = game.Name;
            _rating = _ratingBuilder.Build(rating);
            return ToViewModel();
        }

        public GamePlayerRating ToEntity() {
            return new GamePlayerRating();
        }

        public GamePlayerRating Build(GameRatingViewModel viewModel) {
            return ToEntity();
        }
    }
}
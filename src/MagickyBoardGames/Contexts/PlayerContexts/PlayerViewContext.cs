using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.PlayerContexts
{
    public class PlayerViewContext : IPlayerViewContext {
        private readonly IUserRepository _userRepository;
        private readonly IBuilder<ApplicationUser, PlayerViewModel> _playerBuilder;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IBuilder<GamePlayerRating, GameRatingViewModel> _gameRatingBuilder;

        public PlayerViewContext(IUserRepository userRepository, IBuilder<ApplicationUser, PlayerViewModel> playerBuilder, IBuilder<Game, GameViewModel> gameBuilder, IBuilder<GamePlayerRating, GameRatingViewModel> gameRatingBuilder) {
            _userRepository = userRepository;
            _playerBuilder = playerBuilder;
            _gameBuilder = gameBuilder;
            _gameRatingBuilder = gameRatingBuilder;
        }

        public async Task<PlayerViewViewModel> BuildViewModel(string id) {
            var player = await _userRepository.GetById(id);
            if (player == null)
                return new PlayerViewViewModel();

            return new PlayerViewViewModel {
                Player = _playerBuilder.Build(player),
                Games = GetGames(player) ?? new List<GameViewModel>(),
                GamesRated = GetRatedGames(player) ?? new List<GameRatingViewModel>()
            };
        }

        private IEnumerable<GameViewModel> GetGames(ApplicationUser player) {
            return player.GameOwners?.Select(go => _gameBuilder.Build(go.Game)).ToList();
        }
        private IEnumerable<GameRatingViewModel> GetRatedGames(ApplicationUser player) {
            return player.GamePlayerRatings?
                .Where(gpr => gpr.RatingId != 0)
                .Select(gpr => _gameRatingBuilder.Build(gpr))
                .OrderByDescending(gpr => gpr.Rating.Rate)
                .ThenBy(gpr => gpr.GameName)
                .ToList();
        }
    }
}

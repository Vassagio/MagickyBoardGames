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

        public PlayerViewContext(IUserRepository userRepository, IBuilder<ApplicationUser, PlayerViewModel> playerBuilder, IBuilder<Game, GameViewModel> gameBuilder) {
            _userRepository = userRepository;
            _playerBuilder = playerBuilder;
            _gameBuilder = gameBuilder;
        }

        public async Task<PlayerViewViewModel> BuildViewModel(string id) {
            var player = await _userRepository.GetById(id);
            if (player == null)
                return new PlayerViewViewModel();

            return new PlayerViewViewModel {
                Player = _playerBuilder.Build(player),
                Games = GetGames(player) ?? new List<GameViewModel>()
            };
        }

        private IEnumerable<GameViewModel> GetGames(ApplicationUser player) {
            return player.GameOwners?.Select(go => _gameBuilder.Build(go.Game)).ToList();
        }
    }
}

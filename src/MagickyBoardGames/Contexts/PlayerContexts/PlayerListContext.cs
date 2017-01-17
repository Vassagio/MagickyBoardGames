using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.PlayerContexts
{
    public class PlayerListContext : IPlayerListContext
    {
        private readonly IUserRepository _userRepository;
        private readonly IBuilder<ApplicationUser, PlayerViewModel> _playerBuilder;

        public PlayerListContext(IUserRepository userRepository, IBuilder<ApplicationUser, PlayerViewModel> playerBuilder) {
            _userRepository = userRepository;
            _playerBuilder = playerBuilder;
        }

        public async Task<PlayerListViewModel> BuildViewModel() {
            var players = await _userRepository.GetAll();

            var viewModels = players.Select(player => _playerBuilder.Build(player)).ToList();
            return new PlayerListViewModel {
                Players = viewModels
            };
        }
    }
}

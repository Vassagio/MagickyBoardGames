using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameListContext : IGameListContext {
        private readonly IRepository<Game> _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;

        public GameListContext(IRepository<Game> gameRepository, IBuilder<Game, GameViewModel> gameBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
        }

        public async Task<GameListViewModel> BuildViewModel() {
            var games = await _gameRepository.GetAll();

            var viewModels = games.Select(game => _gameBuilder.Build(game)).ToList();
            return new GameListViewModel {
                Games = viewModels
            };
        }
    }
}

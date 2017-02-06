using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Services;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameSearchContext : IGameSearchContext {
        private const int MAX_GAMES_LOADED = 100;
        private readonly IGameInfoService _gameInfoService;

        public GameSearchContext(IGameInfoService gameInfoService) {
            _gameInfoService = gameInfoService;
        }

        public async Task<ImportSearchViewModel> BuildViewModel(ImportSearchViewModel viewModel) {
            var results = await _gameInfoService.Search(viewModel.Query);
            var gameIds = results.Take(MAX_GAMES_LOADED).Select(r => r.GameId).ToArray();
            viewModel.BoardGames = await _gameInfoService.LoadGames(gameIds);
            return viewModel;
        }
    }
}
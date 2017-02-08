using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameListContext : IGameListContext {
        private readonly IGameRepository _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;

        public GameListContext(IGameRepository gameRepository, IBuilder<Game, GameViewModel> gameBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
        }

        public async Task<GameListViewModel> BuildViewModel(GameListViewModel viewModel = null) {
            var games = await _gameRepository.GetAll();
            viewModel = viewModel ?? new GameListViewModel();
            if (!string.IsNullOrEmpty(viewModel.Filter.Name))
                games = games.Where(g => g.Name.Contains(viewModel.Filter.Name));
            if (!string.IsNullOrEmpty(viewModel.Filter.Description))
                games = games.Where(g => g.Description.Contains(viewModel.Filter.Description));
            if (viewModel.Filter.NumberOfPlayers.HasValue)
                games = games.Where(g => g.MinPlayers <= viewModel.Filter.NumberOfPlayers && g.MaxPlayers >= viewModel.Filter.NumberOfPlayers);

            var viewModels = games.Select(game => _gameBuilder.Build(game)).ToList();
            viewModel.Games = viewModels;
            return viewModel;
        }
    }
}

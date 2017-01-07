using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameViewContext : IGameViewContext {
        private readonly IRepository<Game> _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;

        public GameViewContext(IRepository<Game> gameRepository, IBuilder<Game, GameViewModel> gameBuilder, IBuilder<Category, CategoryViewModel> categoryBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
            _categoryBuilder = categoryBuilder;
        }

        public async Task<GameViewViewModel> BuildViewModel(int id) {
            var game = await _gameRepository.GetBy(id);
            if (game == null)
                return new GameViewViewModel();

            return new GameViewViewModel {
                Game = _gameBuilder.Build(game),
                Categories = GetCategories(game) ?? new List<CategoryViewModel>()
            };
        }

        public async Task Delete(int id) {
            await _gameRepository.Delete(id);
        }

        private IEnumerable<CategoryViewModel> GetCategories(Game game) {
            return game.GameCategories?.Select(gc => _categoryBuilder.Build(gc.Category)).ToList();
        }
    }
}

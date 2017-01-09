using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameSaveContext : IGameSaveContext {
        private readonly IGameRepository _repository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IValidator<GameViewModel> _validator;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;

        public GameSaveContext(IGameRepository repository, IBuilder<Game, GameViewModel> gameBuilder,IValidator<GameViewModel> validator, IBuilder<Category, CategoryViewModel> categoryBuilder) {
            _repository = repository;
            _gameBuilder = gameBuilder;
            _validator = validator;
            _categoryBuilder = categoryBuilder;
        }

        public ValidationResult Validate(GameSaveViewModel viewModel) {
            return _validator.Validate(viewModel.Game);
        }

        public async Task Save(GameSaveViewModel viewModel) {
            var game = _gameBuilder.Build(viewModel.Game);
            var categories = GetCategories(viewModel.SelectedCategories);
            if (viewModel.Game.Id.HasValue)
                await Save(await _repository.GetBy(viewModel.Game.Id.Value), game, categories);
            else
                await Save(await _repository.GetBy(viewModel.Game.Name), game, categories);
        }

        private IEnumerable<Category> GetCategories(IEnumerable<CategoryViewModel> categoryViewModels) {
            return categoryViewModels.Select(cvm => _categoryBuilder.Build(cvm)).ToList();
        }

        private async Task Save(Game found, Game game, IEnumerable<Category> categories) {
            if (found != null)
                await Update(found, game, categories);
            else
                await Insert(game, categories);
        }

        private async Task Update(Game found, Game game, IEnumerable<Category> categories) {
            found.Description = game.Description;
            await _repository.Update(found, categories);
        }

        private async Task Insert(Game game, IEnumerable<Category> categories) {
            await _repository.Add(game, categories);
        }
    }
}
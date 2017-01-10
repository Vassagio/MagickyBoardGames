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
        private readonly IGameRepository _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IValidator<GameSaveViewModel> _validator;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;

        public GameSaveContext(IGameRepository gameRepository, IBuilder<Game, GameViewModel> gameBuilder, IValidator<GameSaveViewModel> validator, ICategoryRepository categoryRepository, IBuilder<Category, CategoryViewModel> categoryBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
            _validator = validator;
            _categoryRepository = categoryRepository;
            _categoryBuilder = categoryBuilder;
        }

        public ValidationResult Validate(GameSaveViewModel viewModel) {
            return _validator.Validate(viewModel);
        }

        public async Task Save(GameSaveViewModel viewModel) {
            var game = _gameBuilder.Build(viewModel.Game);
            var categories = await GetCategories(viewModel.CategoryIds);
            if (viewModel.Game.Id.HasValue)
                await Save(await _gameRepository.GetBy(viewModel.Game.Id.Value), game, categories);
            else
                await Save(await _gameRepository.GetBy(viewModel.Game.Name), game, categories);
        }

        private async Task<IEnumerable<Category>> GetCategories(IEnumerable<int> categoryIds) {
            var categories = new List<Category>();
            foreach (var categoryId in categoryIds) {
                var category = await _categoryRepository.GetBy(categoryId);
                categories.Add(category);
            }
            return categories;
        }

        private async Task Save(Game found, Game game, IEnumerable<Category> categories) {
            if (found != null)
                await Update(found, game, categories);
            else
                await Insert(game, categories);
        }

        private async Task Update(Game found, Game game, IEnumerable<Category> categories) {
            found.Name = game.Name;
            found.Description = game.Description;
            found.MinPlayers = game.MinPlayers;
            found.MaxPlayers = game.MaxPlayers;
            await _gameRepository.Update(found, categories);
        }

        private async Task Insert(Game game, IEnumerable<Category> categories) {
            await _gameRepository.Add(game, categories);
        }

        public async Task<GameSaveViewModel> BuildViewModel() {
            var categories = await _categoryRepository.GetAll();
            var categoryViewModels = categories.Select(category => _categoryBuilder.Build(category)).ToList();
            return new GameSaveViewModel {
                AvailableCategories = categoryViewModels
            };
        }

        public async Task<GameSaveViewModel> BuildViewModel(int id) {
            var viewModel = await BuildViewModel();
            var game = await _gameRepository.GetBy(id);
            if (game == null)
                return viewModel;

            viewModel.Game = _gameBuilder.Build(game);
            viewModel.CategoryIds = BuildCategoryIds(game).ToArray();
            return viewModel;
        }

        private static IEnumerable<int> BuildCategoryIds(Game game) {
            return game.GameCategories.Select(gameCategory => gameCategory.CategoryId).ToList();
        }
    }
}
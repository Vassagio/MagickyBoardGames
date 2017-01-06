using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts
{
    public class CategoryDetailContext : ICategoryDetailContext {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;

        public CategoryDetailContext(IRepository<Category> categoryRepository, IBuilder<Category, CategoryViewModel> categoryBuilder, IBuilder<Game, GameViewModel> gameBuilder) {
            _categoryRepository = categoryRepository;
            _categoryBuilder = categoryBuilder;
            _gameBuilder = gameBuilder;
        }

        public async Task<CategoryDetailViewModel> BuildViewModel(int id) {
            var category = await _categoryRepository.GetBy(id);
            if (category == null)
                return new CategoryDetailViewModel();

            return new CategoryDetailViewModel {
                Category = _categoryBuilder.Build(category),
                Games = GetGames(category) ?? new List<GameViewModel>()
            };
        }

        private IEnumerable<GameViewModel> GetGames(Category category) {
            return category.GameCategories?.Select(gameCategory => _gameBuilder.Build(gameCategory.Game)).ToList();
        }
    }
}

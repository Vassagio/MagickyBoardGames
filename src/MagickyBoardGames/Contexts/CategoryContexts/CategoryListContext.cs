using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts
{
    public class CategoryListContext : ICategoryListContext
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;

        public CategoryListContext(ICategoryRepository categoryRepository, IBuilder<Category, CategoryViewModel> categoryBuilder) {
            _categoryRepository = categoryRepository;
            _categoryBuilder = categoryBuilder;
        }

        public async Task<CategoryListViewModel> BuildViewModel() {
            var categories = await _categoryRepository.GetAll();

            var viewModels = categories.Select(category => _categoryBuilder.Build(category)).ToList();
            return new CategoryListViewModel {
                Categories = viewModels
            };
        }
    }
}

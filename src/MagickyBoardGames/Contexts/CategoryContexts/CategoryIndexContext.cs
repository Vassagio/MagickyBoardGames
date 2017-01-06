using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts
{
    public class CategoryIndexContext : ICategoryIndexContext
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;

        public CategoryIndexContext(IRepository<Category> categoryRepository, IBuilder<Category, CategoryViewModel> categoryBuilder) {
            _categoryRepository = categoryRepository;
            _categoryBuilder = categoryBuilder;
        }

        public async Task<CategoryIndexViewModel> BuildViewModel() {
            var categories = await _categoryRepository.GetAll();

            var viewModels = categories.Select(category => _categoryBuilder.Build(category)).ToList();
            return new CategoryIndexViewModel {
                Categories = viewModels
            };
        }
    }
}

using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public class CategorySaveContext {
        private readonly IRepository<Category> _repository;
        private readonly IBuilder<Category, CategoryViewModel> _builder;

        public CategorySaveContext(IRepository<Category> repository, IBuilder<Category, CategoryViewModel> builder) {
            _repository = repository;
            _builder = builder;
        }
    }
}
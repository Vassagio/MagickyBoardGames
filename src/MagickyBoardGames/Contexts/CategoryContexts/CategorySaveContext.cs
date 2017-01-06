using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public class CategorySaveContext : ICategorySaveContext {
        private readonly IRepository<Category> _repository;
        private readonly IBuilder<Category, CategoryViewModel> _builder;
        private readonly IValidator<CategoryViewModel> _validator;

        public CategorySaveContext(IRepository<Category> repository, IBuilder<Category, CategoryViewModel> builder, IValidator<CategoryViewModel> validator) {
            _repository = repository;
            _builder = builder;
            _validator = validator;
        }

        public ValidationResult Validate(CategoryViewModel viewModel) {
            return _validator.Validate(viewModel);
        }

        public async Task Save(CategoryViewModel viewModel) {
            var entity = _builder.Build(viewModel);
            var found = await _repository.GetBy(entity);
            if (found != null && !viewModel.Id.HasValue)
                return;

            if (found != null)
                await Update(entity);
            else
                await Insert(entity);            
        }

        private async Task Update(Category entity) {
            await _repository.Update(entity);
        }

        private async Task Insert(Category entity) {
            await _repository.Add(entity);
        }
    }
}
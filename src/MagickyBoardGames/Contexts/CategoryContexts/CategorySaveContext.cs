using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public class CategorySaveContext : ICategorySaveContext {
        private readonly ICategoryRepository _repository;
        private readonly IBuilder<Category, CategoryViewModel> _builder;
        private readonly IValidator<CategoryViewModel> _validator;

        public CategorySaveContext(ICategoryRepository repository, IBuilder<Category, CategoryViewModel> builder, IValidator<CategoryViewModel> validator) {
            _repository = repository;
            _builder = builder;
            _validator = validator;
        }

        public ValidationResult Validate(CategoryViewModel viewModel) {
            return _validator.Validate(viewModel);
        }

        public async Task Save(CategoryViewModel viewModel) {
            var entity = _builder.Build(viewModel);
            if (viewModel.Id.HasValue) 
                await Save(await _repository.GetBy(viewModel.Id.Value), entity);            
            else 
                await Save(await _repository.GetBy(viewModel.Description), entity);            
        }

        private async Task Save(Category found, Category entity) {
            if (found != null)
                await Update(found, entity);
            else
                await Insert(entity);
        }

        private async Task Update(Category found, Category entity) {
            found.Description = entity.Description;
            await _repository.Update(found);
        }

        private async Task Insert(Category entity) {
            await _repository.Add(entity);
        }
    }
}
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace MagickyBoardGames.Tests.Contexts.CategoryContexts
{
    public class CategorySaveContextTest
    {
        [Fact]
        public void Initialize_Category_Save_Context() {
            var context = BuildCategorySaveContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public void Validates() {
            var viewModel = new CategoryViewModel();
            var validator = new MockValidator<CategoryViewModel>().ValidateStubbedToReturnValid();
            var context = BuildCategorySaveContext(validator: validator);

            var results = context.Validate(viewModel);

            results.Should().BeOfType<ValidationResult>();
        }

        [Fact]
        public async void Saves_A_New_Record() {
            var category = new Category {
                Description = "Category"
            };
            var viewModel = new CategoryViewModel {
                Description = "Category"
            };
            var repository = new MockRepository<Category>().GetByStubbedToReturn(null);
            var builder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildCategorySaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(category);
            repository.VerifyAddCalled(category);
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Does_Not_Save_When_A_New_Record_Already_Exists() {
            var category = new Category {
                Description = "Category"
            };
            var viewModel = new CategoryViewModel {
                Description = "Category"
            };
            var repository = new MockRepository<Category>().GetByStubbedToReturn(category);
            var builder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildCategorySaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(category);
            repository.VerifyAddNotCalled();
            repository.VerifyUpdateNotCalled();
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Saves_A_New_Record_When_Record_Not_Found() {
            var category = new Category {
                Id = 50,
                Description = "Category"
            };
            var viewModel = new CategoryViewModel {
                Id = 50,
                Description = "Category"
            };
            var repository = new MockRepository<Category>().GetByStubbedToReturn(null);
            var builder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildCategorySaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(category);
            repository.VerifyAddCalled(category);
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found() {
            var category = new Category {
                Id = 60,
                Description = "Category"
            };
            var viewModel = new CategoryViewModel {
                Id = 60,
                Description = "Category"
            };
            var repository = new MockRepository<Category>().GetByStubbedToReturn(category);
            var builder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildCategorySaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(category);
            repository.VerifyUpdateCalled(category);
            builder.VerifyBuildCalled(viewModel);
        }

        private static CategorySaveContext BuildCategorySaveContext(IRepository<Category> repository = null, IBuilder<Category, CategoryViewModel> builder = null, IValidator<CategoryViewModel> validator = null) {
            repository = repository ?? new MockRepository<Category>();
            builder = builder ?? new MockBuilder<Category, CategoryViewModel>();
            validator = validator ?? new MockValidator<CategoryViewModel>();
            return new CategorySaveContext(repository, builder, validator);
        }
    }
}

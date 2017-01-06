using System.Collections.Generic;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.CategoryContexts
{
    public class CategoryListContextTest
    {
        [Fact]
        public void Initializes() {
            var context = BuildCategoryListContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var categoryRepository = new MockRepository<Category>().GetByStubbedToReturn(null);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildCategoryListContext(categoryRepository, categoryBuilder);

            var categoryListViewModel = await context.BuildViewModel();

            categoryListViewModel.Should().BeOfType<CategoryListViewModel>();
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Build_View_Model() {
            var entity = new Category();
            var entities = new List<Category> {entity};
            var viewModel = new CategoryViewModel();
            var repository = new MockRepository<Category>().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(viewModel);
            var context = BuildCategoryListContext(repository, builder);

            var categoryListViewModel = await context.BuildViewModel();

            categoryListViewModel.Should().BeOfType<CategoryListViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        private static CategoryListContext BuildCategoryListContext(IRepository<Category> repository = null, IBuilder<Category, CategoryViewModel> builder = null) {
            repository = repository ?? new MockRepository<Category>();
            builder = builder ?? new MockBuilder<Category, CategoryViewModel>();
            return new CategoryListContext(repository, builder);
        }
    }
}

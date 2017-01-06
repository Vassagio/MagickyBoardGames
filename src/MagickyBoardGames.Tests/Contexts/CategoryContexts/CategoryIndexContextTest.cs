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
    public class CategoryIndexContextTest
    {
        [Fact]
        public void Initialize_Category_Index_Context() {
            var context = BuildCategoryIndexContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var categoryRepository = new MockRepository<Category>().GetByStubbedToReturn(null);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildCategoryIndexContext(categoryRepository, categoryBuilder);

            var categoryIndexViewModel = await context.BuildViewModel();

            categoryIndexViewModel.Should().BeOfType<CategoryIndexViewModel>();
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
            var context = BuildCategoryIndexContext(repository, builder);

            var categoryIndexViewModel = await context.BuildViewModel();

            categoryIndexViewModel.Should().BeOfType<CategoryIndexViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        private static CategoryIndexContext BuildCategoryIndexContext(IRepository<Category> repository = null, IBuilder<Category, CategoryViewModel> builder = null) {
            repository = repository ?? new MockRepository<Category>();
            builder = builder ?? new MockBuilder<Category, CategoryViewModel>();
            return new CategoryIndexContext(repository, builder);
        }
    }
}

using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;

namespace MagickyBoardGames.Tests.Contexts.CategoryContexts
{
    public class CategorySaveContextTest
    {
        [Fact]
        public void Initialize_Category_Create_Context() {
            var context = BuildCategoryCreateContext();
            context.Should().NotBeNull();
        }

        private static CategorySaveContext BuildCategoryCreateContext(IRepository<Category> repository = null, IBuilder<Category, CategoryViewModel> builder = null) {
            repository = repository ?? new MockRepository<Category>();
            builder = builder ?? new MockBuilder<Category, CategoryViewModel>();
            return new CategorySaveContext(repository, builder);
        }
    }
}

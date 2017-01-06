using MagickyBoardCategorys.Builders;
using Xunit;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Tests.Builders
{
    public class CategoryBuilderTest
    {
        [Fact]
        public void View_Model_To_Category() {
            var viewModel = new CategoryViewModel {
                Id = 4,
                Description = "Description",
            };
            var builder = new CategoryBuilder();

            var category = builder.Build(viewModel);

            category.Id.Should().Be(viewModel.Id);
            category.Description.Should().Be(viewModel.Description);
        }

        [Fact]
        public void Category_To_View_Model() {
            var category = new Category {
                Id = 4,
                Description = "Description",
            };
            var builder = new CategoryBuilder();

            var viewModel = builder.Build(category);

            viewModel.Id.Should().Be(category.Id);
            viewModel.Description.Should().Be(category.Description);
        }
    }
}

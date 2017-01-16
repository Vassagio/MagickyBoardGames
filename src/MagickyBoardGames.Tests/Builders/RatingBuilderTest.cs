using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Builders {
    public class RatingBuilderTest {
        [Fact]
        public void View_Model_To_Rating() {
            var viewModel = new RatingViewModel {
                Id = 4,
                Description = "1 - Short - Long"
            };
            var builder = new RatingBuilder();

            var category = builder.Build(viewModel);

            category.Id.Should().Be(viewModel.Id);
            category.Rate.Should().Be(1);
            category.ShortDescription.Should().Be("Short");
            category.Description.Should().Be("Long");
        }

        [Fact]
        public void Rating_To_View_Model() {
            var category = new Rating {
                Id = 4,
                Rate = 1,
                ShortDescription = "Short",
                Description = "Long"
            };
            var builder = new RatingBuilder();

            var viewModel = builder.Build(category);

            viewModel.Id.Should().Be(category.Id);
            viewModel.Description.Should().Be("1 - Short - Long");
        }
    }
}
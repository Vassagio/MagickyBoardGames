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
                Rate = 1, 
                ShortDescription = "Short",
                Description = "Long",
                LongDescription = "1 - Short - Long"
            };
            var builder = new RatingBuilder();

            var rating = builder.Build(viewModel);

            rating.Id.Should().Be(viewModel.Id);
            rating.Rate.Should().Be(1);
            rating.ShortDescription.Should().Be("Short");
            rating.Description.Should().Be("Long");
        }

        [Fact]
        public void Rating_To_View_Model() {
            var rating = new Rating {
                Id = 4,
                Rate = 1,
                ShortDescription = "Short",
                Description = "Long"
            };
            var builder = new RatingBuilder();

            var viewModel = builder.Build(rating);

            viewModel.Id.Should().Be(rating.Id);
            viewModel.Rate.Should().Be(rating.Rate);
            viewModel.ShortDescription.Should().Be(rating.ShortDescription);
            viewModel.Description.Should().Be(rating.Description);
            viewModel.LongDescription.Should().Be("1 - Short - Long");
        }
    }
}
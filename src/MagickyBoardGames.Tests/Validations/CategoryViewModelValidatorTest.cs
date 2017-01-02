using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Validations;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Validations {
    public class CategoryViewModelValidatorTest {
        [Theory]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(30)]
        public void Valid(int length) {
            var viewModel = new CategoryViewModel {
                Description = new string('x', length)
            };
            var validator = new CategoryViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Must_Have_A_Description(string description) {
            var viewModel = new CategoryViewModel {
                Description = description
            };
            var validator = new CategoryViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Must have a description.");
        }

        [Theory]
        [InlineData(31)]
        [InlineData(100)]
        public void Must_Be_Less_Than_Or_Equal_To_30_Characters(int length) {
            var viewModel = new CategoryViewModel {
                Description = new string('x', length)
            };
            var validator = new CategoryViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Must be less than or equal to 30 characters.");
        }
    }
}
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Builders {
    public class OwnerBuilderTest {
        [Fact]
        public void View_Model_To_Owner() {
            var viewModel = new OwnerViewModel {
                Id = "4",
                Name = "Name"
            };
            var builder = new OwnerBuilder();

            var owner = builder.Build(viewModel);

            owner.Id.Should().Be(viewModel.Id);
            owner.UserName.Should().Be(viewModel.Name);
        }

        [Fact]
        public void Owner_To_View_Model() {
            var owner = new ApplicationUser {
                Id = "4",
                UserName = "Name"
            };
            var builder = new OwnerBuilder();

            var viewModel = builder.Build(owner);

            viewModel.Id.Should().Be(owner.Id);
            viewModel.Name.Should().Be(owner.UserName);
        }
    }
}
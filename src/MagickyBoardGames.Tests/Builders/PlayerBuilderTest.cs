using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Builders {
    public class PlayerBuilderTest {
        [Fact]
        public void View_Model_To_Player() {
            var viewModel = new PlayerViewModel {
                Id = "4",
                Name = "Name",
                Email = "Email"
            };
            var builder = new PlayerBuilder();

            var owner = builder.Build(viewModel);

            owner.Id.Should().Be(viewModel.Id);
            owner.UserName.Should().Be(viewModel.Name);
            owner.Email.Should().Be(viewModel.Email);
        }

        [Fact]
        public void Player_To_View_Model() {
            var owner = new ApplicationUser {
                Id = "4",
                UserName = "Name",
                Email = "Email"
            };
            var builder = new PlayerBuilder();

            var viewModel = builder.Build(owner);

            viewModel.Id.Should().Be(owner.Id);
            viewModel.Name.Should().Be(owner.UserName);
            viewModel.Email.Should().Be(owner.Email);
        }
    }
}
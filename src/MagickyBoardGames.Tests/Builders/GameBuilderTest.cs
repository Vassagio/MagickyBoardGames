using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;

namespace MagickyBoardGames.Tests.Builders
{
    public class GameBuilderTest
    {
        [Fact]
        public void View_Model_To_Game() {
            var viewModel = new GameViewModel {
                Id = 4,
                Name = "Name",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10,
                PlayerRange = "1 - 10"
            };
            var builder = BuildGameBuilder();

            var game = builder.Build(viewModel);

            game.Id.Should().Be(viewModel.Id);
            game.Name.Should().Be(viewModel.Name);
            game.Description.Should().Be(viewModel.Description);
            game.MinPlayers.Should().Be(viewModel.MinPlayers);
            game.MaxPlayers.Should().Be(viewModel.MaxPlayers);
        }

        [Fact]
        public void Game_To_View_Model() {           
            var game = new Game {
                Id = 4,
                Name = "Name",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var builder = BuildGameBuilder();

            var viewModel = builder.Build(game);

            viewModel.Id.Should().Be(game.Id);
            viewModel.Name.Should().Be(game.Name);
            viewModel.Description.Should().Be(game.Description);
            viewModel.ShortDescription.Should().Be(game.Description);
            viewModel.MinPlayers.Should().Be(game.MinPlayers);
            viewModel.MaxPlayers.Should().Be(game.MaxPlayers);
            viewModel.PlayerRange.Should().Be("1 - 10");
        }

        [Fact]
        public void Game_To_View_Model_With_Long_Description() {
            var game = new Game {
                Description = new string('x', 1000),
            };
            var builder = BuildGameBuilder();

            var viewModel = builder.Build(game);

            viewModel.Description.Should().Be(game.Description);
            viewModel.ShortDescription.Length.Should().NotBe(game.Description.Length);
        }

        [Theory]
        [InlineData("This is a test.","This is a test.")]
        [InlineData("This is a test.  A really good test.", "This is a test.  A really good test.")]
        [InlineData("This is a test of the national broadcast station that you are currently listening to very, very intently.", "This is a test of the national broadcast station that you are currently listening to very, very intently.")]
        [InlineData("This is a test of the national broadcast station that you are currently listening to very, very intently.  I think you should know this.", "This is a test of the national broadcast station that you are currently listening to very, very intently.")]
        public void Game_To_View_Model_With_Long_Description_Sentences(string description, string expected) {
            var game = new Game {
                Description = description,
            };
            var builder = BuildGameBuilder();

            var viewModel = builder.Build(game);

            viewModel.Description.Should().Be(description);
            viewModel.ShortDescription.Should().Be(expected);
        }

        [Fact]
        public void Game_To_View_Model_With_No_Description() {
            var game = new Game ();
            var builder = BuildGameBuilder();

            var viewModel = builder.Build(game);

            viewModel.Description.Should().Be(game.Description);
            viewModel.ShortDescription.Should().BeEmpty();
        }

        private static GameBuilder BuildGameBuilder(IGamePlayerRatingRepository gamePlayerRatingRepository = null) {
            gamePlayerRatingRepository = gamePlayerRatingRepository ?? new MockGamePlayerRatingRepository();
            return new GameBuilder(gamePlayerRatingRepository);
        }
    }
}

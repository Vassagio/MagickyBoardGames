using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts {
    public class GameContextTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public GameContextTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_Game_Context() {
            var context = BuildGameContext();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IContext<GameViewModel>>();
        }

        [Fact]
        public async void Adds_A_Record() {
            var viewModel = new GameViewModel {
                Name = "Added Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameContext();
            await _fixture.Populate();

            var id = await context.Add(viewModel);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == id);
            expected.Name.Should().Be("Added Game");
            expected.Description.Should().Be("Description");
            expected.MinPlayers.Should().Be(1);
            expected.MaxPlayers.Should().Be(10);
        }

        [Theory]
        [InlineData("Added Game 1", null, 10)]
        [InlineData("Added Game 2", 1, null)]
        [InlineData(null, 1, 10)]
        public void Throws_Exception_When_Adding_An_Invalid_Record(string name, int? minPlayers, int? maxPlayers) {
            var viewModel = new GameViewModel {
                Name = name,
                Description = "Description",
                MinPlayers = minPlayers,
                MaxPlayers = maxPlayers
            };
            var context = BuildGameContext();

            Func<Task> asyncFunction = async () => { await context.Add(viewModel); };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Deletes_A_Record() {
            var game = new Game {
                Id = 1,
                Name = "Deleted Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameContext();
            await _fixture.Populate(game);

            await context.Delete(1);

            _fixture.Db.Games.Any().Should().BeFalse();
        }

        [Fact]
        public void Does_Not_Throw_Exception_When_Deleting_An_Unknown_Record() {
            var context = BuildGameContext();

            Func<Task> asyncFunction = async () => { await context.Delete(100); };
            asyncFunction.ShouldNotThrow();
        }

        [Fact]
        public async void Updates_A_Record() {
            var game = new Game {
                Id = 1,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameContext();
            await _fixture.Populate(game);
            var updated = new GameViewModel {
                Id = 1,
                Name = "Updated Game",
                Description = "We are updated",
                MinPlayers = 2,
                MaxPlayers = 8
            };

            await context.Update(updated);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == 1);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
        }

        [Theory]
        [InlineData("Added Game 1", null, 10)]
        [InlineData("Added Game 2", 1, null)]
        [InlineData(null, 1, 10)]
        public async void Throws_Exception_When_Updating_With_Invalid_Record(string name, int? minPlayers, int? maxPlayers) {
            var game = new Game {
                Id = 1,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameContext();
            await _fixture.Populate(game);

            Func<Task> asyncFunction = async () => {
                var updated = new GameViewModel {
                    Id = 1,
                    Name = name,
                    MinPlayers = minPlayers,
                    MaxPlayers = maxPlayers
                };
                await context.Update(updated);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_Exception_When_Updating_A_Record_That_Doesnt_Exists() {
            var context = BuildGameContext();

            Func<Task> asyncFunction = async () => {
                var updated = new GameViewModel {
                    Id = 1000,
                    Name = "Doesn't Exist",
                    MinPlayers = 1,
                    MaxPlayers = 10
                };
                await context.Update(updated);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Get_All_Records() {
            var game1 = new Game {
                Id = 1,
                Name = "Game 1",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var game2 = new Game {
                Id = 2,
                Name = "Game 2",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameContext();
            await _fixture.Populate(game1, game2);

            var games = await context.GetAll();

            games.Count().Should().Be(2);
        }

        private GameContext BuildGameContext() {
            var gameBuilder = new GameBuilder();
            return new GameContext(_fixture.Db, gameBuilder);
        }
    }
}
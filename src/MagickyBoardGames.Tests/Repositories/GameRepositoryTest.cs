using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MagickyBoardGames.Tests.Repositories {
    public class GameRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public GameRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_Game_Repository() {
            var context = BuildGameRepository();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IRepository<Game>>();
        }

        [Fact]
        public async void Adds_A_Record() {
            var game = new Game {
                Name = "Added Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate();

            var id = await context.Add(game);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == id);
            expected.Name.Should().Be("Added Game");
            expected.Description.Should().Be("Description");
            expected.MinPlayers.Should().Be(1);
            expected.MaxPlayers.Should().Be(10);
        }

        [Fact]
        public async void Adds_A_Record_With_No_Categories() {
            var game = new Game {
                Name = "Added Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate();

            var id = await context.Add(game, new List<Category>());

            var expected = await _fixture.Db.Games.Include(g => g.GameCategories).SingleOrDefaultAsync(g => g.Id == id);
            expected.Name.Should().Be("Added Game");
            expected.Description.Should().Be("Description");
            expected.MinPlayers.Should().Be(1);
            expected.MaxPlayers.Should().Be(10);
            expected.GameCategories.Should().BeEmpty();
        }

        [Fact]
        public async void Adds_A_Record_With_Categories() {
            var game = new Game {
                Name = "Added Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var category1 = new Category {
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            var categories = new List<Category> {
                category1,
                category2
            };
            var context = BuildGameRepository();
            await _fixture.Populate(category1, category2);

            var id = await context.Add(game, categories);

            var expected = await _fixture.Db.Games.Include(g => g.GameCategories).SingleOrDefaultAsync(g => g.Id == id);
            expected.Name.Should().Be("Added Game");
            expected.Description.Should().Be("Description");
            expected.MinPlayers.Should().Be(1);
            expected.MaxPlayers.Should().Be(10);
            expected.GameCategories.Count().Should().Be(2);            
        }

        [Fact]
        public void Throws_Exception_When_Adding_An_Invalid_Record() {
            var entity = new Game {
                Name = null,
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();

            Func<Task> asyncFunction = async () => { await context.Add(entity); };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Deletes_A_Record() {
            var game = new Game {
                Id = 666,
                Name = "Deleted Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game);

            await context.Delete(666);

            _fixture.Db.Games.Any().Should().BeFalse();
        }

        [Fact]
        public void Does_Not_Throw_Exception_When_Deleting_An_Unknown_Record() {
            var context = BuildGameRepository();

            Func<Task> asyncFunction = async () => { await context.Delete(100); };
            asyncFunction.ShouldNotThrow();
        }

        [Fact]
        public async void Updates_A_Record() {
            var game = new Game {
                Id = 999,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game);
            game.Name = "Updated Game";
            game.Description = "We are updated";
            game.MinPlayers = 2;
            game.MaxPlayers = 8;

            await context.Update(game);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == 999);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
        }

        [Fact]
        public async void Updates_A_Record_With_No_Categories() {
            var game = new Game {
                Id = 999,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game);
            game.Name = "Updated Game";
            game.Description = "We are updated";
            game.MinPlayers = 2;
            game.MaxPlayers = 8;

            await context.Update(game, new List<Category>());

            var expected = await _fixture.Db.Games.Include(g => g.GameCategories).SingleOrDefaultAsync(g => g.Id == 999);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
            expected.GameCategories.Should().BeEmpty();
        }

        [Fact]
        public async void Updates_A_Record_With_Categories_When_Category_Already_Exists() {
            var game = new Game {
                Id = 999,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var category1 = new Category {
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            var categories = new List<Category> {
                category1,
                category2
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game);
            await _fixture.Populate(category1, category2);
            await _fixture.Populate(new GameCategory { Id = 500, GameId = 999, CategoryId = 1 });
            game.Name = "Updated Game";
            game.Description = "We are updated";
            game.MinPlayers = 2;
            game.MaxPlayers = 8;

            await context.Update(game, categories);

            var expected = await _fixture.Db.Games.Include(g => g.GameCategories).SingleOrDefaultAsync(g => g.Id == 999);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
            expected.GameCategories.Count.Should().Be(2);
        }

        [Fact]
        public async void Updates_A_Record_With_Adding_And_Removing_Categories() {
            var game = new Game {
                Id = 999,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var category1 = new Category {
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            var gameCategory1 = new GameCategory {
                Id = 300,
                GameId = 999,
                CategoryId = 1
            };   
            var context = BuildGameRepository();
            await _fixture.Populate(game);
            await _fixture.Populate(category1, category2);
            await _fixture.Populate(gameCategory1);
            game.Name = "Updated Game";
            game.Description = "We are updated";
            game.MinPlayers = 2;
            game.MaxPlayers = 8;

            await context.Update(game, new List<Category> {category2});

            var expected = await _fixture.Db.Games.Include(g => g.GameCategories).SingleOrDefaultAsync(g => g.Id == 999);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
            expected.GameCategories.Count.Should().Be(1);
            expected.GameCategories.First().CategoryId.Should().Be(2);
        }

        [Fact]        
        public async void Throws_Exception_When_Updating_With_Invalid_Record() {
            var game = new Game {
                Id = 9991,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game);

            Func<Task> asyncFunction = async () => {
                game.Name = null;
                await context.Update(game);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_Exception_When_Updating_A_Record_That_Doesnt_Exists() {
            var context = BuildGameRepository();

            Func<Task> asyncFunction = async () => {
                var updated = new Game {
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
                Id = 111,
                Name = "Game 1",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var game2 = new Game {
                Id = 222,
                Name = "Game 2",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game1, game2);

            var games = await context.GetAll();

            games.Count().Should().Be(2);
        }

        [Fact]
        public async void Get_By_Id() {
            var game1 = new Game {
                Id = 111,
                Name = "Game 1",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var game2 = new Game {
                Id = 222,
                Name = "Game 2",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game1, game2);

            var category = await context.GetBy(222);

            category.Should().Be(game2);
        }


        [Theory]
        [InlineData("Game 1")]
        [InlineData("game 1")]
        [InlineData("GAME 1")]
        public async void Get_By_Unique_Key(string name) {
            var game1 = new Game {
                Id = 111,
                Name = "Game 1",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var game2 = new Game {
                Id = 222,
                Name = "Game 2",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var context = BuildGameRepository();
            await _fixture.Populate(game1, game2);

            var game = await context.GetBy(name);

            game.Should().Be(game1);
        }

        private GameRepository BuildGameRepository() {
            return new GameRepository(_fixture.Db);
        }
    }
}
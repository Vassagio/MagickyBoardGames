using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
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
                Id = 1,
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
        public async void Adds_A_Record_With_Children() {
            var game = new Game {
                Name = "Added Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var categories = new List<Category> {
                new Category()
            };
            var owners = new List<ApplicationUser> {
                new ApplicationUser(),
                new ApplicationUser()
            };
            var gameCategoryRepository = new MockGameCategoryRepository();
            var gameOwnerRepository = new MockGameOwnerRepository();
            var context = BuildGameRepository(gameCategoryRepository, gameOwnerRepository);
            await _fixture.Populate();
            
            var id = await context.Add(game, categories, owners);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == id);
            expected.Name.Should().Be("Added Game");
            expected.Description.Should().Be("Description");
            expected.MinPlayers.Should().Be(1);
            expected.MaxPlayers.Should().Be(10);          
            gameCategoryRepository.VerifyAdjustCalled(id, categories);
            gameOwnerRepository.VerifyAdjustCalled(id, owners);
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
        public async void Updates_A_Record_With_Children() {
            var game = new Game {
                Id = 999,
                Name = "Original Game",
                Description = "Description",
                MinPlayers = 1,
                MaxPlayers = 10
            };
            var categories = new List<Category> {
                new Category(),
                new Category(),
                new Category()
            };
            var owners = new List<ApplicationUser> {
                new ApplicationUser(),                
            };
            var gameCategoryRepository = new MockGameCategoryRepository();
            var gameOwnerRepository = new MockGameOwnerRepository();
            var context = BuildGameRepository(gameCategoryRepository, gameOwnerRepository);
            await _fixture.Populate(game);
            game.Name = "Updated Game";
            game.Description = "We are updated";
            game.MinPlayers = 2;
            game.MaxPlayers = 8;

            await context.Update(game, categories, owners);

            var expected = await _fixture.Db.Games.SingleOrDefaultAsync(g => g.Id == 999);
            expected.Name.Should().Be("Updated Game");
            expected.Description.Should().Be("We are updated");
            expected.MinPlayers.Should().Be(2);
            expected.MaxPlayers.Should().Be(8);
            gameCategoryRepository.VerifyAdjustCalled(999, categories);
            gameOwnerRepository.VerifyAdjustCalled(999, owners);
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

        private GameRepository BuildGameRepository(IGameCategoryRepository gameCategoryRepository = null, IGameOwnerRepository gameOwnerRepository = null) {
            gameCategoryRepository = gameCategoryRepository ?? new MockGameCategoryRepository();
            gameOwnerRepository = gameOwnerRepository ?? new MockGameOwnerRepository();
            return new GameRepository(_fixture.Db, gameCategoryRepository, gameOwnerRepository);
        }
    }
}
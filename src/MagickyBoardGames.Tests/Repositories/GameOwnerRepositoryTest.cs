using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Xunit;

namespace MagickyBoardGames.Tests.Repositories {
    public class GameOwnerRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public GameOwnerRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initializes() {
            var repository = BuildGameOwnerRepository();
            repository.Should().NotBeNull();
            repository.Should().BeAssignableTo<IGameOwnerRepository>();
        }

        [Fact]
        public async void Adjust_Adds() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var owner1 = new ApplicationUser {
                Id = "1",
                UserName = "Owner 1"
            };
            var owner2 = new ApplicationUser {
                Id = "2",
                UserName = "Owner 2"
            };
            await _fixture.Populate(game);
            await _fixture.Populate(owner1, owner2);
            var repository = BuildGameOwnerRepository();

            await repository.Adjust(1,
                                    new List<ApplicationUser> {
                                        owner2
                                    });

            var actual = _fixture.Db.GameOwners.SingleOrDefault(gc => gc.GameId == 1);
            actual.Should().NotBeNull();
            actual.OwnerId.Should().Be("2");
        }

        [Fact]
        public async void Adjust_Removes() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var owner1 = new ApplicationUser {
                Id = "1",
                UserName = "Owner 1"
            };
            var owner2 = new ApplicationUser {
                Id = "2",
                UserName = "Owner 2"
            };
            var gameOwner1 = new GameOwner {
                Id = 1,
                GameId = 1,
                OwnerId = "1"
            };
            var gameOwner2 = new GameOwner {
                Id = 2,
                GameId = 1,
                OwnerId = "2"
            };
            await _fixture.Populate(game);
            await _fixture.Populate(owner1, owner2);
            await _fixture.Populate(gameOwner1, gameOwner2);
            var repository = BuildGameOwnerRepository();

            await repository.Adjust(1,
                                    new List<ApplicationUser> {
                                        owner2
                                    });

            var actual = _fixture.Db.GameOwners.SingleOrDefault(gc => gc.GameId == 1);
            actual.Should().NotBeNull();
            actual.OwnerId.Should().Be("2");
        }

        private GameOwnerRepository BuildGameOwnerRepository() {
            return new GameOwnerRepository(_fixture.Db);
        }
    }
}
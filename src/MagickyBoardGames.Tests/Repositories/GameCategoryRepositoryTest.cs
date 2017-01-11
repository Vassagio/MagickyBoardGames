using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Repositories;
using Xunit;
using FluentAssertions;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Tests.Repositories
{
    public class GameCategoryRepositoryTest: IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public GameCategoryRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }
        [Fact]
        public void Initializes() {
            var repository = BuildGameCategoryRepository();
            repository.Should().NotBeNull();
            repository.Should().BeAssignableTo<IGameCategoryRepository>();
        }

        [Fact]
        public async void Adjust_Adds() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var category1 = new Category {
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            await _fixture.Populate(game);
            await _fixture.Populate(category1, category2);
            var repository = BuildGameCategoryRepository();

            await repository.Adjust(1, new List<Category> {category2});

            var actual = _fixture.Db.GameCategories.SingleOrDefault(gc => gc.GameId == 1);
            actual.Should().NotBeNull();
            actual.CategoryId.Should().Be(2);
        }

        [Fact]
        public async void Adjust_Removes() {
            var game = new Game {
                Id = 1,
                Name = "Game"
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
                Id = 1,
                GameId = 1,
                CategoryId = 1
            };
            var gameCategory2 = new GameCategory {
                Id = 2,
                GameId = 1,
                CategoryId = 2
            };
            await _fixture.Populate(game);
            await _fixture.Populate(category1, category2);
            await _fixture.Populate(gameCategory1, gameCategory2);
            var repository = BuildGameCategoryRepository();

            await repository.Adjust(1, new List<Category> { category2 });

            var actual = _fixture.Db.GameCategories.SingleOrDefault(gc => gc.GameId == 1);
            actual.Should().NotBeNull();
            actual.CategoryId.Should().Be(2);
        }

        private GameCategoryRepository BuildGameCategoryRepository() {
            return new GameCategoryRepository(_fixture.Db);
        }
    }
}

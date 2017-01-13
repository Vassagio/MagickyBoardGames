using MagickyBoardGames.Repositories;
using Xunit;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Tests.Repositories
{
    public class GamePlayerRatingRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public GamePlayerRatingRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initializes() {
            var repository = BuildGamePlayerRatingRepository();
            repository.Should().NotBeNull();
            repository.Should().BeAssignableTo<IGamePlayerRatingRepository>();
        }

        [Fact]
        public async void Save_A_New_Rating() {
            var game = new Game { Id = 1 };
            var player = new ApplicationUser { Id = "1" };
            var rating = new Rating { Id = 1 };
            await _fixture.Populate(game);
            await _fixture.Populate(player);
            await _fixture.Populate(rating);
            var repository = BuildGamePlayerRatingRepository();

            await repository.Save(game.Id, player.Id, rating.Id);

            var actual = _fixture.Db.GamePlayerRatings.SingleOrDefault(gpr => gpr.GameId == 1 && gpr.PlayerId == "1");
            actual.Should().NotBeNull();
            actual.Rating.Id.Should().Be(1);
        }

        [Fact]
        public async void Save_Over_An_Existing_Rating() {
            var game = new Game { Id = 1 };
            var player = new ApplicationUser { Id = "1" };
            var rating2 = new Rating { Id = 2 };
            var rating3 = new Rating { Id = 3 };
            var gamePlayerRating = new GamePlayerRating {
                GameId = game.Id,
                PlayerId = player.Id,
                RatingId = rating2.Id
            };
            await _fixture.Populate(game);
            await _fixture.Populate(player);
            await _fixture.Populate(rating2, rating3);
            await _fixture.Populate(gamePlayerRating);
            var repository = BuildGamePlayerRatingRepository();

            await repository.Save(game.Id, player.Id, rating3.Id);

            var actual = _fixture.Db.GamePlayerRatings.SingleOrDefault(gpr => gpr.GameId == 1 && gpr.PlayerId == "1");
            actual.Should().NotBeNull();
            actual.Rating.Id.Should().Be(3);
        }

        private GamePlayerRatingRepository BuildGamePlayerRatingRepository() {
            return new GamePlayerRatingRepository(_fixture.Db);
        }
    }
}

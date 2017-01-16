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
        public async void Get_By_Game_Id_And_Player_Id() {
            var gamePlayerRating1 = new GamePlayerRating {
                GameId = 1,
                PlayerId = "1",
                RatingId = 1
            };
            var gamePlayerRating2 = new GamePlayerRating {
                GameId = 2,
                PlayerId = "2",
                RatingId = 2
            };
            var context = BuildGamePlayerRatingRepository();
            await _fixture.Populate(gamePlayerRating1, gamePlayerRating2);

            var gamePlayerRating = await context.GetBy(1, "1");

            gamePlayerRating.Should().Be(gamePlayerRating1);
        }

        [Fact]
        public async void Get_By_Returns_Null_When_Game_Id_And_Player_Id_Doesnt_Exits() {
            var gamePlayerRating1 = new GamePlayerRating {
                GameId = 1,
                PlayerId = "1",
                RatingId = 1
            };
            var gamePlayerRating2 = new GamePlayerRating {
                GameId = 2,
                PlayerId = "2",
                RatingId = 2
            };
            var context = BuildGamePlayerRatingRepository();
            await _fixture.Populate(gamePlayerRating1, gamePlayerRating2);

            var gamePlayerRating = await context.GetBy(3, "3");

            gamePlayerRating.Should().BeNull();
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
        public async void Save_A_New_Rating_For_A_Different_Player() {
            var game = new Game { Id = 1 };
            var player1 = new ApplicationUser { Id = "1" };
            var player2 = new ApplicationUser { Id = "2" };
            var rating2 = new Rating { Id = 2 };
            var rating3 = new Rating { Id = 3 };
            var gamePlayerRating = new GamePlayerRating {
                GameId = game.Id,
                PlayerId = player1.Id,
                RatingId = rating2.Id
            };
            await _fixture.Populate(game);
            await _fixture.Populate(player1, player2);
            await _fixture.Populate(rating2, rating3);
            await _fixture.Populate(gamePlayerRating);
            var repository = BuildGamePlayerRatingRepository();

            await repository.Save(game.Id, player2.Id, rating3.Id);

            var actual1 = _fixture.Db.GamePlayerRatings.SingleOrDefault(gpr => gpr.GameId == 1 && gpr.PlayerId == "1");
            var actual2 = _fixture.Db.GamePlayerRatings.SingleOrDefault(gpr => gpr.GameId == 1 && gpr.PlayerId == "2");
            actual1.Should().NotBeNull();
            actual1.Rating.Id.Should().Be(2);
            actual2.Should().NotBeNull();
            actual2.Rating.Id.Should().Be(3);
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

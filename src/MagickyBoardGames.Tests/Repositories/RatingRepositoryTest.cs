using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Xunit;

namespace MagickyBoardGames.Tests.Repositories {
    public class RatingRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public RatingRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_Rating_Repository() {
            var context = BuildRatingRepository();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IRatingRepository>();
        }

        [Fact]
        public async void Get_All_Records() {
            var rating1 = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "1",
                Description = "Rating 1"
            };
            var rating2 = new Rating {
                Id = 2,
                Rate = 2,
                ShortDescription = "2",
                Description = "Rating 2"
            };
            var context = BuildRatingRepository();
            await _fixture.Populate(rating1, rating2);

            var ratings = await context.GetAll();

            ratings.Count().Should().Be(2);
        }

        [Fact]
        public async void Get_By_Id() {
            var rating1 = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "1",
                Description = "Rating 1"
            };
            var rating2 = new Rating {
                Id = 2,
                Rate = 2,
                ShortDescription = "2",
                Description = "Rating 2"
            };
            var context = BuildRatingRepository();
            await _fixture.Populate(rating1, rating2);

            var user = await context.GetBy(2);

            user.Should().Be(rating2);
        }

        [Theory]
        [InlineData("Rating 1")]
        [InlineData("rating 1")]
        [InlineData("RATING 1")]
        public async void Get_By_Unique_Key(string description) {
            var rating1 = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "1",
                Description = "Rating 1"
            };
            var rating2 = new Rating {
                Id = 2,
                Rate = 2,
                ShortDescription = "2",
                Description = "Rating 2"
            };
            var context = BuildRatingRepository();
            await _fixture.Populate(rating1, rating2);

            var user = await context.GetBy(description);

            user.Should().Be(rating1);
        }

        private RatingRepository BuildRatingRepository() {
            return new RatingRepository(_fixture.Db);
        }
    }
}
using FluentAssertions;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts.Rating;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.RatingContexts {
    public class RatingContextLoaderTest {
        [Fact]
        public void Initializes() {
            var contextLoader = BuildContextLoader();
            contextLoader.Should().NotBeNull();
            contextLoader.Should().BeAssignableTo<IRatingContextLoader>();
        }

        [Fact]
        public void Loads_Rating_List_Context() {
            var ratingListContext = new MockRatingListContext();
            var contextLoader = BuildContextLoader(ratingListContext);

            var context = contextLoader.LoadRatingListContext();

            context.Should().Be(ratingListContext);
        }

        [Fact]
        public void Loads_Rating_View_Context() {
            var ratingViewContext = new MockRatingViewContext();
            var contextLoader = BuildContextLoader(ratingViewContext: ratingViewContext);

            var context = contextLoader.LoadRatingViewContext();

            context.Should().Be(ratingViewContext);
        }

        private static RatingContextLoader BuildContextLoader(IRatingListContext ratingListContext = null, IRatingViewContext ratingViewContext = null) {
            ratingListContext = ratingListContext ?? new MockRatingListContext();
            ratingViewContext = ratingViewContext ?? new MockRatingViewContext();
            return new RatingContextLoader(ratingListContext, ratingViewContext);
        }
    }
}
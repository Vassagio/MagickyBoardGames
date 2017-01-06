using MagickyBoardGames.Contexts;
using FluentAssertions;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts
{
    public class ContextLoaderTest
    {
        [Fact]
        public void Initializes() {
            var contextLoader = BuildContextLoader();
            contextLoader.Should().NotBeNull();
            contextLoader.Should().BeAssignableTo<IContextLoader>();
        }

        [Fact]
        public void Loads_Category_Index_Context() {
            var categoryIndexContext = new MockCategoryIndexContext();
            var contextLoader = BuildContextLoader(categoryIndexContext);

            var context = contextLoader.LoadCategoryIndexContext();

            context.Should().Be(categoryIndexContext);
        }

        [Fact]
        public void Loads_Category_Detail_Context() {
            var categoryDetailContext = new MockCategoryDetailContext();
            var contextLoader = BuildContextLoader(categoryDetailContext: categoryDetailContext);

            var context = contextLoader.LoadCategoryDetailContext();

            context.Should().Be(categoryDetailContext);
        }

        private static ContextLoader BuildContextLoader(ICategoryIndexContext categoryIndexContext = null, ICategoryDetailContext categoryDetailContext = null) {
            categoryIndexContext = categoryIndexContext ?? new MockCategoryIndexContext();
            categoryDetailContext = categoryDetailContext ?? new MockCategoryDetailContext();
            return new ContextLoader(categoryIndexContext, categoryDetailContext);
        }
    }
}

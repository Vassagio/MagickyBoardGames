﻿using MagickyBoardGames.Contexts;
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
        public void Loads_Category_List_Context() {
            var categoryListContext = new MockCategoryListContext();
            var contextLoader = BuildContextLoader(categoryListContext);

            var context = contextLoader.LoadCategoryListContext();

            context.Should().Be(categoryListContext);
        }

        [Fact]
        public void Loads_Category_View_Context() {
            var categoryViewContext = new MockCategoryViewContext();
            var contextLoader = BuildContextLoader(categoryViewContext: categoryViewContext);

            var context = contextLoader.LoadCategoryViewContext();

            context.Should().Be(categoryViewContext);
        }

        [Fact]
        public void Loads_Category_Save_Context() {
            var categorySaveContext = new MockCategorySaveContext();
            var contextLoader = BuildContextLoader(categorySaveContext: categorySaveContext);

            var context = contextLoader.LoadCategorySaveContext();

            context.Should().Be(categorySaveContext);
        }

        private static ContextLoader BuildContextLoader(ICategoryListContext categoryListContext = null, ICategoryViewContext categoryViewContext = null, ICategorySaveContext categorySaveContext = null) {
            categoryListContext = categoryListContext ?? new MockCategoryListContext();
            categoryViewContext = categoryViewContext ?? new MockCategoryViewContext();
            categorySaveContext = categorySaveContext ?? new MockCategorySaveContext();
            return new ContextLoader(categoryListContext, categoryViewContext, categorySaveContext);
        }
    }
}

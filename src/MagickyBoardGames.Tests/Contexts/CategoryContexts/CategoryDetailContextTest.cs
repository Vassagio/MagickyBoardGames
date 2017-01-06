using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.CategoryContexts {
    public class CategoryDetailContextTest {
        [Fact]
        public void Initialize_Category_Detail_Context() {
            var context = BuildCategoryDetailContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var categoryRepository = new MockRepository<Category>().GetByStubbedToReturn(null);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildCategoryDetailContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryDetailViewModel = await context.BuildViewModel(1000);

            categoryDetailViewModel.Should().BeOfType<CategoryDetailViewModel>();
            categoryRepository.VerifyGetByCalled(1000);
            categoryBuilder.VerifyBuildNotCalled();
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_No_Associated_Games() {
            var category = new Category {
                Id = 1,
                Description = "Category"
            };
            var categoryViewModel = new CategoryViewModel {
                Id = 1,
                Description = "Category"
            };
            var categoryRepository = new MockRepository<Category>().GetByStubbedToReturn(category);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildCategoryDetailContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryDetailViewModel = await context.BuildViewModel(category.Id);

            categoryDetailViewModel.Should().BeOfType<CategoryDetailViewModel>();
            categoryDetailViewModel.Category.Should().Be(categoryViewModel);
            categoryDetailViewModel.Games.Should().BeEmpty();
            categoryRepository.VerifyGetByCalled(category.Id);
            categoryBuilder.VerifyBuildCalled(category);
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_Games() {
            var game = new Game {
                Id = 1,
                Name = "Game",
            };
            var category = new Category {
                Id = 1,
                Description = "Category",
            };
            category.GameCategories = new List<GameCategory> {
                new GameCategory {
                    Id = 1,
                    GameId = game.Id,
                    Game = game,
                    CategoryId = category.Id,
                    Category= category
                }
            };
            var categoryViewModel = new CategoryViewModel {
                Id = category.Id,
                Description = category.Description
            };
            var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var categoryRepository = new MockRepository<Category>().GetByStubbedToReturn(category);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var context = BuildCategoryDetailContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryDetailViewModel = await context.BuildViewModel(category.Id);

            categoryDetailViewModel.Should().BeOfType<CategoryDetailViewModel>();
            categoryDetailViewModel.Category.Should().Be(categoryViewModel);
            categoryDetailViewModel.Games.Count().Should().Be(1);
            categoryDetailViewModel.Games.Should().Contain(gameViewModel);
            categoryRepository.VerifyGetByCalled(category.Id);
            categoryBuilder.VerifyBuildCalled(category);
            gameBuilder.VerifyBuildCalled(game);
        }

        private static CategoryDetailContext BuildCategoryDetailContext(IRepository<Category> categoryRepository = null, IBuilder<Category, CategoryViewModel> categoryBuilder = null, IBuilder<Game, GameViewModel> gameBuilder = null) {
            categoryRepository = categoryRepository ?? new MockRepository<Category>();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            return new CategoryDetailContext(categoryRepository, categoryBuilder, gameBuilder);
        }
    }
}

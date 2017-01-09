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
    public class CategoryViewContextTest {
        [Fact]
        public void Initialize_Category_View_Context() {
            var context = BuildCategoryViewContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(null);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildCategoryViewContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryViewViewModel = await context.BuildViewModel(1000);

            categoryViewViewModel.Should().BeOfType<CategoryViewViewModel>();
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
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(category);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildCategoryViewContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryViewViewModel = await context.BuildViewModel(category.Id);

            categoryViewViewModel.Should().BeOfType<CategoryViewViewModel>();
            categoryViewViewModel.Category.Should().Be(categoryViewModel);
            categoryViewViewModel.Games.Should().BeEmpty();
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
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(category);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var context = BuildCategoryViewContext(categoryRepository, categoryBuilder, gameBuilder);

            var categoryViewViewModel = await context.BuildViewModel(category.Id);

            categoryViewViewModel.Should().BeOfType<CategoryViewViewModel>();
            categoryViewViewModel.Category.Should().Be(categoryViewModel);
            categoryViewViewModel.Games.Count().Should().Be(1);
            categoryViewViewModel.Games.Should().Contain(gameViewModel);
            categoryRepository.VerifyGetByCalled(category.Id);
            categoryBuilder.VerifyBuildCalled(category);
            gameBuilder.VerifyBuildCalled(game);
        }

        [Fact]
        public async void Does_Not_Throw_Exception_When_Deleting_Nonexistant_Record() {
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(null);
            var context = BuildCategoryViewContext(categoryRepository);

            await context.Delete(1000);

            categoryRepository.VerifyDeleteCalled(1000);
        }

        [Fact]
        public async void Delete_A_Record() {
            var category = new Category {
                Id = 500,
                Description = "Category"
            };
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(category);
            var context = BuildCategoryViewContext(categoryRepository);

            await context.Delete(500);

            categoryRepository.VerifyDeleteCalled(500);
        }
        private static CategoryViewContext BuildCategoryViewContext(ICategoryRepository categoryRepository = null, IBuilder<Category, CategoryViewModel> categoryBuilder = null, IBuilder<Game, GameViewModel> gameBuilder = null) {
            categoryRepository = categoryRepository ?? new MockCategoryRepository();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            return new CategoryViewContext(categoryRepository, categoryBuilder, gameBuilder);
        }
    }
}

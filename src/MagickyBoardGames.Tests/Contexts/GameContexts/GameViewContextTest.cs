using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameViewContextTest {
        [Fact]
        public void Initialize_Game_View_Context() {
            var context = BuildGameViewContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildGameViewContext(gameRepository, gameBuilder, categoryBuilder);

            var gameViewViewModel = await context.BuildViewModel(1000);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameRepository.VerifyGetByCalled(1000);
            gameBuilder.VerifyBuildNotCalled();
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_No_Associated_Games() {
            var game = new Game {
                Id = 1,
                Description = "Game"
            };
            var gameViewModel = new GameViewModel {
                Id = 1,
                Description = "Game"
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildGameViewContext(gameRepository, gameBuilder, categoryBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.Categories.Should().BeEmpty();
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            categoryBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_Games() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var category = new Category {
                Id = 1,
                Description = "Category"
            };
            game.GameCategories = new List<GameCategory> {
                new GameCategory {
                    Id = 1,
                    GameId = game.Id,
                    Game = game,
                    CategoryId = category.Id,
                    Category = category
                }
            };
            var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var categoryViewModel = new CategoryViewModel {
                Id = category.Id,
                Description = category.Description
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var context = BuildGameViewContext(gameRepository, gameBuilder, categoryBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.Categories.Count().Should().Be(1);
            gameViewViewModel.Categories.Should().Contain(categoryViewModel);
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            categoryBuilder.VerifyBuildCalled(category);
        }

        [Fact]
        public async void Does_Not_Throw_Exception_When_Deleting_Nonexistant_Record() {
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var context = BuildGameViewContext(gameRepository);

            await context.Delete(1000);

            gameRepository.VerifyDeleteCalled(1000);
        }

        [Fact]
        public async void Delete_A_Record() {
            var game = new Game {
                Id = 500,
                Description = "Game"
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var context = BuildGameViewContext(gameRepository);

            await context.Delete(500);

            gameRepository.VerifyDeleteCalled(500);
        }

        private static GameViewContext BuildGameViewContext(IGameRepository gameRepository = null,
                                                            IBuilder<Game, GameViewModel> gameBuilder = null,
                                                            IBuilder<Category, CategoryViewModel> categoryBuilder = null) {
            gameRepository = gameRepository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            return new GameViewContext(gameRepository, gameBuilder, categoryBuilder);
        }
    }
}
using System.Collections.Generic;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameSaveContextTest {
        [Fact]
        public void Initialize_Game_Save_Context() {
            var context = BuildGameSaveContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public void Validates() {
            var viewModel = new GameSaveViewModel();
            var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnValid();
            var context = BuildGameSaveContext(validator: validator);

            var results = context.Validate(viewModel);

            results.Should().BeOfType<ValidationResult>();
        }

        [Fact]
        public async void Saves_A_New_Record_Without_Categories() {
            var game = new Game {
                Name = "Game"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, categoryBuilder: categoryBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game, new List<Category>());
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Saves_A_New_Record_With_Categories() {
            var game = new Game {
                Name = "Game"
            };
            var category = new Category {
                Description = "Category 1"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var categoryViewModel = new CategoryViewModel {
                Id = 1,
                Description = "Category 1"
            };
            var gameSaveViewModel = new GameSaveViewModel {
                Game = gameViewModel,
                SelectedCategories = new List<CategoryViewModel> {
                    categoryViewModel
                }
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildGameSaveContext(repository, gameBuilder, categoryBuilder: categoryBuilder);

            await context.Save(gameSaveViewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game, new List<Category> { category });
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildCalled(categoryViewModel);
        }

        [Fact]
        public async void Does_Not_Save_When_A_New_Record_Already_Exists() {
            var game = new Game {
                Name = "Game"
            };  
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, gameBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddNotCalled();
            repository.VerifyUpdateCalled(game, new List<Category>());
            gameBuilder.VerifyBuildCalled(gameViewModel);
        }

        [Fact]
        public async void Saves_A_New_Record_When_Record_Not_Found() {
            var game = new Game {
                Id = 50,
                Name = "Game"
            };
            var gameViewModel = new GameViewModel {
                Id = 50,
                Name = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, gameBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(50);
            repository.VerifyGetByNameNotCalled();
            repository.VerifyAddCalled(game, new List<Category>());
            gameBuilder.VerifyBuildCalled(gameViewModel);
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found_Without_Categories() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };
            var gameViewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, categoryBuilder: categoryBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(60);
            repository.VerifyGetByNameNotCalled();
            repository.VerifyUpdateCalled(game, new List<Category>());
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found_With_Categories() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };
            var category = new Category {
                Description = "Category 1"
            };
            var gameViewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var categoryViewModel = new CategoryViewModel {
                Id = 1,
                Description = "Category 1"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                SelectedCategories = new List<CategoryViewModel> {
                    categoryViewModel
                }
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(game);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(category);
            var context = BuildGameSaveContext(repository, builder, categoryBuilder: categoryBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(60);
            repository.VerifyGetByNameNotCalled();
            repository.VerifyUpdateCalled(game, new List<Category> { category});
            builder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildCalled(categoryViewModel);
        }

        private static GameSaveContext BuildGameSaveContext(IGameRepository repository = null, IBuilder<Game, GameViewModel> gameBuilder = null, IValidator<GameViewModel> validator = null, IBuilder<Category, CategoryViewModel> categoryBuilder = null) {
            repository = repository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            validator = validator ?? new MockValidator<GameViewModel>();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            return new GameSaveContext(repository, gameBuilder, validator, categoryBuilder);
        }
    }
}

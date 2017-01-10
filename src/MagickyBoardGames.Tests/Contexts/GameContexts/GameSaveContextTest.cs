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
using MagickyBoardGames.Validations;

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
            var validator = new MockValidator<GameSaveViewModel>().ValidateStubbedToReturnValid();
            var context = BuildGameSaveContext(validator: validator);

            var results = context.Validate(viewModel);

            results.Should().BeOfType<ValidationResult>();
        }

        [Fact]
        public async void Builds_A_View_Model() {
            var category = new Category {
                Id = 1,
                Description = "Category"
            };
            var categories = new List<Category> {
                category
            };
            var categoryViewModel = new CategoryViewModel {
                Id = 1,
                Description = "Category"
            };
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var context = BuildGameSaveContext(categoryRepository: categoryRepository, categoryBuilder: categoryBuilder);

            var viewModel = await context.BuildViewModel();

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> {categoryViewModel});
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category);
        }

        [Fact]
        public async void Builds_A_View_Model_With_Id_That_Doesnt_Exist() {
            var category = new Category {
                Id = 1,
                Description = "Category"
            };
            var categories = new List<Category> {
                category
            };
            var categoryViewModel = new CategoryViewModel {
                Id = 1,
                Description = "Category"
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository, categoryBuilder: categoryBuilder);

            var viewModel = await context.BuildViewModel(33);

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> { categoryViewModel });
            gameRepository.VerifyGetByCalled(33);
            gameBuilder.VerifyBuildNotCalled();
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category);
        }

        [Fact]
        public async void Builds_A_View_Model_With_Id() {
            var category1 = new Category {
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            var game = new Game {
                Id = 33,
                Name = "Game",
                GameCategories = new List<GameCategory> {
                    new GameCategory {
                        Id = 1, 
                        GameId = 33,
                        CategoryId = 2
                    }
                }
            };
            var gameViewModel = new GameViewModel {
                Id = 33,
                Name = "Game"
            };
            var categories = new List<Category> {
                category1,
                category2
            };
            var categoryViewModel1 = new CategoryViewModel {
                Id = 1,
                Description = "Category 1"
            };
            var categoryViewModel2 = new CategoryViewModel {
                Id = 2,
                Description = "Category 2"
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel1, categoryViewModel2);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository, categoryBuilder: categoryBuilder);

            var viewModel = await context.BuildViewModel(33);

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> { categoryViewModel1, categoryViewModel2 });
            viewModel.CategoryIds.Should().BeEquivalentTo(new[] { 2 });
            gameRepository.VerifyGetByCalled(33);
            gameBuilder.VerifyBuildCalled(game);
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category1);
            categoryBuilder.VerifyBuildCalled(category2);
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
            var gameSaveViewModel = new GameSaveViewModel {
                Game = gameViewModel,
                CategoryIds = new[] {
                    1
                }
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(category);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository);

            await context.Save(gameSaveViewModel);

            gameRepository.VerifyGetByCalled(game.Name);
            gameRepository.VerifyGetByIdNotCalled();
            gameRepository.VerifyAddCalled(game, new List<Category> { category });
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryRepository.VerifyGetByCalled(1);
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
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                CategoryIds = new[] {
                    1
                }
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryRepository = new MockCategoryRepository().GetByStubbedToReturn(category);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository);

            await context.Save(viewModel);

            gameRepository.VerifyGetByCalled(60);
            gameRepository.VerifyGetByNameNotCalled();
            gameRepository.VerifyUpdateCalled(game, new List<Category> { category});
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryRepository.VerifyGetByCalled(1);
        }

        private static GameSaveContext BuildGameSaveContext(IGameRepository gameRepository = null, IBuilder<Game, GameViewModel> gameBuilder = null, IValidator<GameSaveViewModel> validator = null, ICategoryRepository categoryRepository = null, IBuilder < Category, CategoryViewModel> categoryBuilder = null) {
            gameRepository = gameRepository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            validator = validator ?? new MockValidator<GameSaveViewModel>();
            categoryRepository = categoryRepository ?? new MockCategoryRepository();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            return new GameSaveContext(gameRepository, gameBuilder, validator, categoryRepository, categoryBuilder);
        }
    }
}

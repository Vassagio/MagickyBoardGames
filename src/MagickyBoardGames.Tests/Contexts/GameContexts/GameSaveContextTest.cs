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
            var category = new Category();
            var categories = new List<Category> {
                category
            };
            var categoryViewModel = new CategoryViewModel();
            var owner = new ApplicationUser();
            var owners = new List<ApplicationUser> {
                owner
            };
            var ownerViewModel = new OwnerViewModel();
            var rating = new Rating();
            var ratings = new List<Rating> {
                rating
            };
            var ratingViewModel = new RatingViewModel();
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var userRepository = new MockUserRepository().GetAllStubbedToReturn(owners);
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>().BuildStubbedToReturn(ownerViewModel);
            var ratingRepository = new MockRatingRepository().GetAllStubbedToReturn(ratings);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var context = BuildGameSaveContext(categoryRepository: categoryRepository, categoryBuilder: categoryBuilder, userRepository: userRepository, ownerBuilder: ownerBuilder, ratingRepository: ratingRepository, ratingBuilder: ratingBuilder);

            var viewModel = await context.BuildViewModel();

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> {categoryViewModel});
            viewModel.AvailableOwners.Should().BeEquivalentTo(new List<OwnerViewModel> { ownerViewModel });
            viewModel.AvailableRatings.Should().BeEquivalentTo(new List<RatingViewModel> { ratingViewModel });
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category);
            userRepository.VerifyGetAllCalled();
            ownerBuilder.VerifyBuildCalled(owner);
            ratingRepository.VerifyGetAllCalled();
            ratingBuilder.VerifyBuildCalled(rating);
        }

        [Fact]
        public async void Builds_A_View_Model_With_Id_That_Doesnt_Exist() {
            var category = new Category();
            var categories = new List<Category> {
                category
            };
            var categoryViewModel = new CategoryViewModel();
            var owner = new ApplicationUser();
            var owners = new List<ApplicationUser> {
                owner
            };
            var ownerViewModel = new OwnerViewModel();
            var rating = new Rating();
            var ratings = new List<Rating> {
                rating
            };
            var ratingViewModel = new RatingViewModel();
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel);
            var userRepository = new MockUserRepository().GetAllStubbedToReturn(owners);
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>().BuildStubbedToReturn(ownerViewModel);
            var ratingRepository = new MockRatingRepository().GetAllStubbedToReturn(ratings);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository, categoryBuilder: categoryBuilder, userRepository: userRepository, ownerBuilder: ownerBuilder, ratingRepository: ratingRepository, ratingBuilder: ratingBuilder);

            var viewModel = await context.BuildViewModel(33, "1");

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> { categoryViewModel });
            viewModel.AvailableOwners.Should().BeEquivalentTo(new List<OwnerViewModel> { ownerViewModel });
            viewModel.AvailableRatings.Should().BeEquivalentTo(new List<RatingViewModel> { ratingViewModel });
            gameRepository.VerifyGetByCalled(33);
            gameBuilder.VerifyBuildNotCalled();
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category);
            userRepository.VerifyGetAllCalled();
            ownerBuilder.VerifyBuildCalled(owner);
            ratingRepository.VerifyGetAllCalled();
            ratingBuilder.VerifyBuildCalled(rating);
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
            var categories = new List<Category> {
                category1,
                category2
            };
            var owner1 = new ApplicationUser {
                Id = "1",
                UserName = "User Name 1"
            };
            var owner2 = new ApplicationUser {
                Id = "2",
                UserName = "User Name 2"
            };
            var owner3 = new ApplicationUser {
                Id = "3",
                UserName = "User Name 3"
            };
            var owners = new List<ApplicationUser> {
                owner1,
                owner2,
                owner3
            };
            var rating1 = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "Rate 1",
                Description = "Rating 1"
            };
            var rating2 = new Rating {
                Id = 2,
                Rate = 2,
                ShortDescription = "Rate 2",
                Description = "Rating 2"
            };
            var ratings = new List<Rating> {
                rating1,
                rating2
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
                },
                GameOwners = new List<GameOwner> {
                    new GameOwner {
                        Id = 1, 
                        GameId = 33,
                        OwnerId = "3"
                    }
                },
                GamePlayerRatings = new List<GamePlayerRating> {
                    new GamePlayerRating {
                        Id = 1,
                        GameId = 33,
                        PlayerId = "3",
                        RatingId = 1
                    }
                }
            };
            var gameViewModel = new GameViewModel {
                Id = 33,
                Name = "Game"
            };            
            var categoryViewModel1 = new CategoryViewModel {
                Id = 1,
                Description = "Category 1"
            };
            var categoryViewModel2 = new CategoryViewModel {
                Id = 2,
                Description = "Category 2"
            };
            var ownerViewModel1 = new OwnerViewModel {
                Id = "1",
                Name = "User Name 1"
            };
            var ownerViewModel2 = new OwnerViewModel {
                Id = "2",
                Name = "User Name 2"
            };
            var ownerViewModel3 = new OwnerViewModel {
                Id = "3",
                Name = "User Name 3"
            };
            var ratingViewModel1 = new RatingViewModel {
                Id = 1,
                Description = "1 - Rate 1 - Rating 1"
            };
            var ratingViewModel2 = new RatingViewModel {
                Id = 2,
                Description = "2 - Rate 2 - Rating 2"
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var categoryRepository = new MockCategoryRepository().GetAllStubbedToReturn(categories);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>().BuildStubbedToReturn(categoryViewModel1, categoryViewModel2);
            var userRepository = new MockUserRepository().GetAllStubbedToReturn(owners);
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>().BuildStubbedToReturn(ownerViewModel1, ownerViewModel2, ownerViewModel3);
            var ratingRepository = new MockRatingRepository().GetAllStubbedToReturn(ratings);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel1, ratingViewModel2);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, categoryRepository: categoryRepository, categoryBuilder: categoryBuilder, userRepository: userRepository, ownerBuilder: ownerBuilder, ratingRepository: ratingRepository, ratingBuilder: ratingBuilder);

            var viewModel = await context.BuildViewModel(33, "3");

            viewModel.Should().BeOfType<GameSaveViewModel>();
            viewModel.AvailableCategories.Should().BeEquivalentTo(new List<CategoryViewModel> { categoryViewModel1, categoryViewModel2 });
            viewModel.AvailableOwners.Should().BeEquivalentTo(new List<OwnerViewModel> { ownerViewModel1, ownerViewModel2, ownerViewModel3 });
            viewModel.AvailableRatings.Should().BeEquivalentTo(new List<RatingViewModel> { ratingViewModel1, ratingViewModel2 });
            viewModel.CategoryIds.Should().BeEquivalentTo(new[] { 2 });
            viewModel.OwnerIds.Should().BeEquivalentTo("3");
            viewModel.RatingId.Should().Be(1);
            gameRepository.VerifyGetByCalled(33);
            gameBuilder.VerifyBuildCalled(game);
            categoryRepository.VerifyGetAllCalled();
            categoryBuilder.VerifyBuildCalled(category1);
            categoryBuilder.VerifyBuildCalled(category2);
            userRepository.VerifyGetAllCalled();
            ownerBuilder.VerifyBuildCalled(owner1);
            ownerBuilder.VerifyBuildCalled(owner2);
            ownerBuilder.VerifyBuildCalled(owner3);
            ratingRepository.VerifyGetAllCalled();
            ratingBuilder.VerifyBuildCalled(rating1);
            ratingBuilder.VerifyBuildCalled(rating2);
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
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, categoryBuilder: categoryBuilder);

            await context.Save(viewModel);            

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser>(), 0, "1" );
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Saves_A_New_Record_Without_Owners() {
            var game = new Game {
                Name = "Game"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, ownerBuilder: ownerBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser>(), 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            ownerBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Saves_A_New_Record_Without_Ratings() {
            var game = new Game {
                Name = "Game"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, ratingBuilder: ratingBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser>(), 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            ratingBuilder.VerifyBuildNotCalled();
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
                UserId = "1",
                RatingId = 0,
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
            gameRepository.VerifyAddCalled(game, new List<Category> { category }, new List<ApplicationUser>(), 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryRepository.VerifyGetByCalled(1);
        }

        [Fact]
        public async void Saves_A_New_Record_With_Owners() {
            var game = new Game {
                Name = "Game"
            };
            var owner = new ApplicationUser {
                UserName = "User Name"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var gameSaveViewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0,
                OwnerIds = new[] {
                    "1"
                }
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(owner);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, userRepository: userRepository);

            await context.Save(gameSaveViewModel);

            gameRepository.VerifyGetByCalled(game.Name);
            gameRepository.VerifyGetByIdNotCalled();
            gameRepository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser> {owner}, 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            userRepository.VerifyGetByIdCalled("1");
        }

        [Fact]
        public async void Saves_A_New_Record_With_Ratings() {
            var game = new Game {
                Name = "Game"
            };
            var gameViewModel = new GameViewModel {
                Name = "Game"
            };
            var gameSaveViewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 1
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(gameRepository, gameBuilder);

            await context.Save(gameSaveViewModel);

            gameRepository.VerifyGetByCalled(game.Name);
            gameRepository.VerifyGetByIdNotCalled();
            gameRepository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser>(), 1, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
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
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, gameBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game.Name);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddNotCalled();
            repository.VerifyUpdateCalled(game, new List<Category>(), new List<ApplicationUser>(), 0, "1");
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
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, gameBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(50);
            repository.VerifyGetByNameNotCalled();
            repository.VerifyAddCalled(game, new List<Category>(), new List<ApplicationUser>(), 0 ,"1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found_Without_Children() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };
            var gameViewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0
            };
            var repository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var categoryBuilder = new MockBuilder<Category, CategoryViewModel>();
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>();
            var context = BuildGameSaveContext(repository, gameBuilder, categoryBuilder: categoryBuilder, ownerBuilder: ownerBuilder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(60);
            repository.VerifyGetByNameNotCalled();
            repository.VerifyUpdateCalled(game, new List<Category>(), new List<ApplicationUser>(), 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryBuilder.VerifyBuildNotCalled();
            ownerBuilder.VerifyBuildNotCalled();
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
                UserId = "1",
                RatingId = 0,
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
            gameRepository.VerifyUpdateCalled(game, new List<Category> { category}, new List<ApplicationUser>(), 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            categoryRepository.VerifyGetByCalled(1);
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found_With_Owners() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };
            var owner = new ApplicationUser {
                UserName = "User Name 1"
            };
            var gameViewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 0,
                OwnerIds = new[] {
                    "1"
                }
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(owner);
            var context = BuildGameSaveContext(gameRepository, gameBuilder, userRepository: userRepository);

            await context.Save(viewModel);

            gameRepository.VerifyGetByCalled(60);
            gameRepository.VerifyGetByNameNotCalled();
            gameRepository.VerifyUpdateCalled(game, new List<Category>(), new List<ApplicationUser> {owner}, 0, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
            userRepository.VerifyGetByIdCalled("1");
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found_With_Rating() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };            
            var gameViewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var viewModel = new GameSaveViewModel {
                Game = gameViewModel,
                UserId = "1",
                RatingId = 1                
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(gameRepository, gameBuilder);

            await context.Save(viewModel);

            gameRepository.VerifyGetByCalled(60);
            gameRepository.VerifyGetByNameNotCalled();
            gameRepository.VerifyUpdateCalled(game, new List<Category>(), new List<ApplicationUser>(), 1, "1");
            gameBuilder.VerifyBuildCalled(gameViewModel);
        }

        private static GameSaveContext BuildGameSaveContext(IGameRepository gameRepository = null, 
                                                            IBuilder<Game, GameViewModel> gameBuilder = null, 
                                                            IValidator<GameSaveViewModel> validator = null, 
                                                            ICategoryRepository categoryRepository = null, 
                                                            IBuilder < Category, CategoryViewModel> categoryBuilder = null,
                                                            IUserRepository userRepository = null,
                                                            IBuilder<ApplicationUser, OwnerViewModel> ownerBuilder = null,
                                                            IRatingRepository ratingRepository = null,
                                                            IBuilder<Rating, RatingViewModel> ratingBuilder = null) {
            gameRepository = gameRepository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            validator = validator ?? new MockValidator<GameSaveViewModel>();
            categoryRepository = categoryRepository ?? new MockCategoryRepository();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            userRepository = userRepository ?? new MockUserRepository();
            ownerBuilder = ownerBuilder ?? new MockBuilder<ApplicationUser, OwnerViewModel>();
            ratingRepository = ratingRepository ?? new MockRatingRepository();
            ratingBuilder = ratingBuilder ?? new MockBuilder<Rating, RatingViewModel>();
            return new GameSaveContext(gameRepository, gameBuilder, validator, categoryRepository, categoryBuilder, userRepository, ownerBuilder, ratingRepository, ratingBuilder);
        }
    }
}

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
        public async void Returns_View_Model_With_No_Associated_Users() {
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
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>();
            var context = BuildGameViewContext(gameRepository, gameBuilder, ownerBuilder: ownerBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.Owners.Should().BeEmpty();
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            ownerBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_No_Associated_Ratings() {
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
            var context = BuildGameViewContext(gameRepository, gameBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.PlayerRatings.Should().BeEmpty();
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
        }

        [Fact]
        public async void Returns_View_Model_With_Categories() {
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
        public async void Returns_View_Model_With_Owners() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var owner = new ApplicationUser{
                Id = "1",
                UserName = "User Name"
            };
            game.GameOwners = new List<GameOwner> {
                new GameOwner{
                    Id = 1,
                    GameId = game.Id,
                    Game = game,
                    OwnerId = owner.Id,
                    Owner = owner
                }
            };
            var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var ownerViewModel = new OwnerViewModel {
                Id = owner.Id,
                Name = owner.UserName
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var ownerBuilder = new MockBuilder<ApplicationUser, OwnerViewModel>().BuildStubbedToReturn(ownerViewModel);
            var context = BuildGameViewContext(gameRepository, gameBuilder, ownerBuilder: ownerBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.Owners.Count().Should().Be(1);
            gameViewViewModel.Owners.Should().Contain(ownerViewModel);
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            ownerBuilder.VerifyBuildCalled(owner);
        }

        [Fact]
        public async void Returns_View_Model_With_Ratings() {
            var game = new Game {
                Id = 1,
                Name = "Game"
            };
            var rating = new Rating {
                Id = 1,
                Description = "Rating"
            };
            var player = new ApplicationUser {
                Id = "1",
                UserName = "User"
            };
            var gamePlayerRating = new GamePlayerRating{
                Id = 1,
                GameId = game.Id,
                Game = game,
                PlayerId = player.Id,
                Player = player,
                RatingId = rating.Id,
                Rating = rating
            };
            game.GamePlayerRatings = new List<GamePlayerRating> {
                gamePlayerRating
            };
            var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var ratingViewModel = new RatingViewModel {
                Id = rating.Id,
                Description = rating.Description
            };
            var playerRatingViewModel = new PlayerRatingViewModel {
                PlayerName = player.UserName,
                Rating = ratingViewModel
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var playerRatingBuilder = new MockBuilder<GamePlayerRating, PlayerRatingViewModel>().BuildStubbedToReturn(playerRatingViewModel);
            var context = BuildGameViewContext(gameRepository, gameBuilder, playerRatingBuilder: playerRatingBuilder);

            var gameViewViewModel = await context.BuildViewModel(game.Id);

            gameViewViewModel.Should().BeOfType<GameViewViewModel>();
            gameViewViewModel.Game.Should().Be(gameViewModel);
            gameViewViewModel.PlayerRatings.Count().Should().Be(1);
            gameViewViewModel.PlayerRatings.First().PlayerName.Should().Be(player.UserName);
            gameViewViewModel.PlayerRatings.First().Rating.Should().Be(ratingViewModel);
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            playerRatingBuilder.VerifyBuildCalled(gamePlayerRating);
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
                                                            IBuilder<Category, CategoryViewModel> categoryBuilder = null,
                                                            IBuilder<ApplicationUser, OwnerViewModel> ownerBuilder = null,
                                                            IBuilder<GamePlayerRating, PlayerRatingViewModel> playerRatingBuilder = null
                                                            ) {
            gameRepository = gameRepository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            categoryBuilder = categoryBuilder ?? new MockBuilder<Category, CategoryViewModel>();
            ownerBuilder = ownerBuilder ?? new MockBuilder<ApplicationUser, OwnerViewModel>();
            playerRatingBuilder = playerRatingBuilder ?? new MockBuilder<GamePlayerRating, PlayerRatingViewModel>();
            return new GameViewContext(gameRepository, gameBuilder, categoryBuilder, ownerBuilder, playerRatingBuilder);
        }
    }
}
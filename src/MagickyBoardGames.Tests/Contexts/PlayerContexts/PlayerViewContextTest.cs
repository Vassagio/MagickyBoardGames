using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.PlayerContexts {
    public class PlayerViewContextTest {
        [Fact]
        public void Initialize_Player_View_Context() {
            var context = BuildPlayerViewContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var userRepository = new MockUserRepository().GetByStubbedToReturn(null);
            var playerBuilder = new MockBuilder<ApplicationUser, PlayerViewModel>();
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildPlayerViewContext(userRepository, playerBuilder, gameBuilder);

            var playerViewViewModel = await context.BuildViewModel("1000");

            playerViewViewModel.Should().BeOfType<PlayerViewViewModel>();
            userRepository.VerifyGetByIdCalled("1000");
            playerBuilder.VerifyBuildNotCalled();
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_No_Associated_Games() {
            var player = new ApplicationUser {
                Id = "1",
                UserName = "Player"
            };
            var playerViewModel = new PlayerViewModel {
                Id = "1",
                Name = "Player"
            };
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(player);
            var playerBuilder = new MockBuilder<ApplicationUser, PlayerViewModel>().BuildStubbedToReturn(playerViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildPlayerViewContext(userRepository, playerBuilder, gameBuilder);

            var playerViewViewModel = await context.BuildViewModel(player.Id);

            playerViewViewModel.Should().BeOfType<PlayerViewViewModel>();
            playerViewViewModel.Player.Should().Be(playerViewModel);
            playerViewViewModel.Games.Should().BeEmpty();
            userRepository.VerifyGetByIdCalled(player.Id);
            playerBuilder.VerifyBuildCalled(player);
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_Games() {
            var game = new Game {
                Id = 1,
                Name = "Game",
            };
            var owner = new ApplicationUser {
                Id = "1",
                UserName = "Player",
                Email = "Email"
            };
            owner.GameOwners = new List<GameOwner> {
                new GameOwner {
                    Id = 1,
                    GameId = game.Id,
                    Game = game,
                    OwnerId = owner.Id,
                    Owner= owner
                }
            };
            var playerViewModel = new PlayerViewModel {
                Id = owner.Id,
                Name = owner.UserName,
                Email = owner.Email
            }; 
             var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(owner);
            var playerBuilder = new MockBuilder<ApplicationUser, PlayerViewModel>().BuildStubbedToReturn(playerViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var context = BuildPlayerViewContext(userRepository, playerBuilder, gameBuilder);

            var playerViewViewModel = await context.BuildViewModel(owner.Id);

            playerViewViewModel.Should().BeOfType<PlayerViewViewModel>();
            playerViewViewModel.Player.Should().Be(playerViewModel);
            playerViewViewModel.Games.Count().Should().Be(1);
            playerViewViewModel.Games.Should().Contain(gameViewModel);
            userRepository.VerifyGetByIdCalled(owner.Id);
            playerBuilder.VerifyBuildCalled(owner);
            gameBuilder.VerifyBuildCalled(game);
        }

        [Fact]
        public async void Returns_View_Model_With_Rated_Games_Ordered_By_Rate_Descending() {
            var game = new Game {
                Id = 1,
                Name = "Game",
            };
            var owner = new ApplicationUser {
                Id = "1",
                UserName = "Player",
                Email = "Email"
            };
            var rating0 = new Rating {
                Id = 0,
                Rate = 0,
                ShortDescription = "Short0",
                Description = "Long0"
            };
            var rating1 = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "Short1",
                Description = "Long1"
            };
            var rating2 = new Rating {
                Id = 2,
                Rate = 2,
                ShortDescription = "Short2",
                Description = "Long2"
            };
            var ratingViewModel0 = new RatingViewModel {
                Id = rating0.Id,
                Rate = rating0.Rate,
                ShortDescription = rating0.ShortDescription,
                Description = rating0.Description
            };
            var ratingViewModel1 = new RatingViewModel {
                Id = rating1.Id,
                Rate = rating1.Rate,
                ShortDescription = rating1.ShortDescription,
                Description = rating1.Description
            };
            var ratingViewModel2 = new RatingViewModel {
                Id = rating2.Id,
                Rate = rating2.Rate,
                ShortDescription = rating2.ShortDescription,
                Description = rating2.Description
            };
            var gamePlayerRating0 = new GamePlayerRating {
                Id = 3,
                GameId = game.Id,
                Game = game,
                RatingId = rating0.Id,
                Rating = rating0
            };
            var gamePlayerRating1 = new GamePlayerRating {
                Id = 1,
                GameId = game.Id,
                Game = game,
                RatingId = rating1.Id,
                Rating= rating1
            };
            var gamePlayerRating2 = new GamePlayerRating {
                Id = 2,
                GameId = game.Id,
                Game = game,
                RatingId = rating2.Id,
                Rating = rating2
            };
            owner.GamePlayerRatings = new List<GamePlayerRating> {
                gamePlayerRating1, 
                gamePlayerRating2,
                gamePlayerRating0
            };
            var playerViewModel = new PlayerViewModel {
                Id = owner.Id,
                Name = owner.UserName,
                Email = owner.Email
            };
            var gameRatingViewModel1 = new GameRatingViewModel {
                GameName = game.Name,
                Rating = ratingViewModel1
            };
            var gameRatingViewModel2 = new GameRatingViewModel {
                GameName = game.Name,
                Rating = ratingViewModel2
            };
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(owner);
            var playerBuilder = new MockBuilder<ApplicationUser, PlayerViewModel>().BuildStubbedToReturn(playerViewModel);
            var gameRatingBuilder = new MockBuilder<GamePlayerRating, GameRatingViewModel>().BuildStubbedToReturn(gameRatingViewModel1, gameRatingViewModel2);
            var context = BuildPlayerViewContext(userRepository, playerBuilder, gameRatingBuilder: gameRatingBuilder);

            var playerViewViewModel = await context.BuildViewModel(owner.Id);

            playerViewViewModel.Should().BeOfType<PlayerViewViewModel>();
            playerViewViewModel.Player.Should().Be(playerViewModel);
            playerViewViewModel.GamesRated.Count().Should().Be(2);
            playerViewViewModel.GamesRated.First().Should().Be(gameRatingViewModel2);
            playerViewViewModel.GamesRated.Last().Should().Be(gameRatingViewModel1);
            userRepository.VerifyGetByIdCalled(owner.Id);
            playerBuilder.VerifyBuildCalled(owner);
            gameRatingBuilder.VerifyBuildCalled(gamePlayerRating1);
            gameRatingBuilder.VerifyBuildCalled(gamePlayerRating2);
            gameRatingBuilder.VerifyBuildNotCalled(gamePlayerRating0);
        }        

        private static PlayerViewContext BuildPlayerViewContext(IUserRepository userRepository = null, IBuilder<ApplicationUser, PlayerViewModel> playerBuilder = null, IBuilder<Game, GameViewModel> gameBuilder = null, IBuilder<GamePlayerRating, GameRatingViewModel> gameRatingBuilder = null) {
            userRepository = userRepository ?? new MockUserRepository();
            playerBuilder = playerBuilder ?? new MockBuilder<ApplicationUser, PlayerViewModel>();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            gameRatingBuilder = gameRatingBuilder ?? new MockBuilder<GamePlayerRating, GameRatingViewModel>();
            return new PlayerViewContext(userRepository, playerBuilder, gameBuilder, gameRatingBuilder);
        }
    }
}

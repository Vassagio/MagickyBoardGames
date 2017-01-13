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

        private static PlayerViewContext BuildPlayerViewContext(IUserRepository userRepository = null, IBuilder<ApplicationUser, PlayerViewModel> playerBuilder = null, IBuilder<Game, GameViewModel> gameBuilder = null) {
            userRepository = userRepository ?? new MockUserRepository();
            playerBuilder = playerBuilder ?? new MockBuilder<ApplicationUser, PlayerViewModel>();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            return new PlayerViewContext(userRepository, playerBuilder, gameBuilder);
        }
    }
}

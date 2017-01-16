using System.Collections.Generic;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameRateContextTest {
        [Fact]
        public void Initialize_Game_Rate_Context() {
            var context = BuildGameRateContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Builds_A_View_Model_With_Game_Id_And_Player_Id() {
            var game = new Game {
                Id = 4
            };
            var rating = new Rating {
                Id = 43
            };
            var player = new ApplicationUser {
                Id = "12"
            };
            var gamePlayerRating = new GamePlayerRating {
                GameId = game.Id,
                RatingId = rating.Id,
                PlayerId = player.Id
            };
            game.GamePlayerRatings = new List<GamePlayerRating> {
                gamePlayerRating
            };
            var gameViewModel = new GameViewModel {
                Id = game.Id
            };
            var ratingViewModel = new RatingViewModel {
                Id = rating.Id
            };
            var playerRatingViewModel = new PlayerRatingViewModel {
                PlayerName = player.UserName,
                Rating = ratingViewModel
            };
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var ratingRepository = new MockRatingRepository().GetAllStubbedToReturn(new List<Rating> { rating });
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var gamePlayerRatingRepository = new MockGamePlayerRatingRepository().GetByStubbedToReturn(gamePlayerRating);
            var playerRatingBuilder = new MockBuilder<GamePlayerRating, PlayerRatingViewModel>().BuildStubbedToReturn(playerRatingViewModel);
            var context = BuildGameRateContext(gameRepository, gameBuilder, ratingRepository, ratingBuilder, gamePlayerRatingRepository, playerRatingBuilder);

            var viewModel = await context.BuildViewModel(game.Id, player.Id);

            viewModel.Should().BeAssignableTo<GameRateViewModel>();
            gameRepository.VerifyGetByCalled(game.Id);
            gameBuilder.VerifyBuildCalled(game);
            ratingRepository.VerifyGetAllCalled();
            ratingBuilder.VerifyBuildCalled(rating);
            gamePlayerRatingRepository.VerifyGetByCalled(game.Id, player.Id);
            playerRatingBuilder.VerifyBuildCalled(gamePlayerRating);
        }

        [Fact]
        public async void Does_Not_Save_Record_When_Game_Doesnt_Exist() {
            var gameRateViewModel = new GameRateViewModel();
            var gamePlayerRatingRepository = new MockGamePlayerRatingRepository();
            var context = BuildGameRateContext(gamePlayerRatingRepository: gamePlayerRatingRepository);

            await context.Save(gameRateViewModel);

            gamePlayerRatingRepository.VerifySaveNotCalled();
        }

        [Fact]
        public async void Does_Not_Save_Record_When_GameId_Doesnt_Exist() {
            var gameRateViewModel = new GameRateViewModel {
                Game = new GameViewModel()
            };
            var gamePlayerRatingRepository = new MockGamePlayerRatingRepository();
            var context = BuildGameRateContext(gamePlayerRatingRepository: gamePlayerRatingRepository);

            await context.Save(gameRateViewModel);

            gamePlayerRatingRepository.VerifySaveNotCalled();
        }

        [Fact]
        public async void Save_Record() {
            var gameViewModel = new GameViewModel {
                Id = 12
            };
            var gameRateViewModel = new GameRateViewModel {
                Game = gameViewModel,
                UserId = "22",
                RatingId = 2
            };
            var gamePlayerRatingRepository = new MockGamePlayerRatingRepository();
            var context = BuildGameRateContext(gamePlayerRatingRepository: gamePlayerRatingRepository);

            await context.Save(gameRateViewModel);

            gamePlayerRatingRepository.VerifySaveCalled(12, "22", 2);
        }


        private static GameRateContext BuildGameRateContext(IGameRepository gameRepository = null,
                                                            IBuilder<Game, GameViewModel> gameBuilder = null,
                                                            IRatingRepository ratingRepository = null,
                                                            IBuilder<Rating, RatingViewModel> ratingBuilder = null,
                                                            IGamePlayerRatingRepository gamePlayerRatingRepository = null, 
                                                            IBuilder<GamePlayerRating, PlayerRatingViewModel> playerRatingBuilder = null) {
            gameRepository = gameRepository ?? new MockGameRepository();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            ratingRepository = ratingRepository ?? new MockRatingRepository();
            ratingBuilder = ratingBuilder ?? new MockBuilder<Rating, RatingViewModel>();
            gamePlayerRatingRepository = gamePlayerRatingRepository ?? new MockGamePlayerRatingRepository();
            playerRatingBuilder = playerRatingBuilder ?? new MockBuilder<GamePlayerRating, PlayerRatingViewModel>();
            return new GameRateContext(gameRepository, gameBuilder, ratingRepository, ratingBuilder, gamePlayerRatingRepository, playerRatingBuilder);
        }
    }
}
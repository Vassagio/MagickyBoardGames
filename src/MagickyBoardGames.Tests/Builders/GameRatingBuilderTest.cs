using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Builders {
    public class GameRatingBuilderTest {
        [Fact]
        public void View_Model_To_Player() {
            var viewModel = new GameRatingViewModel();
            var builder = BuildGameRatingBuilder();

            var entity = builder.Build(viewModel);

            entity.Should().BeOfType<GamePlayerRating>();
        }


        [Fact]
        public void Player_To_View_Model() {
            var game = new Game {
                Id = 10,
                Name = "Great Game"
            };
            var rating = new Rating {
                Id = 44,
                Rate = 5
            };
            var ratingViewModel = new RatingViewModel {
                Id = rating.Id,
                Rate = rating.Rate
            };
            var gamePlayerRating = new GamePlayerRating {
                Game = game,
                GameId = game.Id,
                Rating = rating,
                RatingId = rating.Id
            };

            var ratingRepository = new MockRatingRepository().GetByIdStubbedToReturn(rating);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(game);
            var builder = BuildGameRatingBuilder(ratingRepository, ratingBuilder, gameRepository);

            var viewModel = builder.Build(gamePlayerRating);

            viewModel.Id.Should().Be(game.Id);
            viewModel.GameName.Should().Be(game.Name);
            viewModel.Rating.Should().Be(ratingViewModel);
            ratingRepository.VerifyGetByCalled(rating.Id);
            ratingBuilder.VerifyBuildCalled(rating);
            gameRepository.VerifyGetByCalled(game.Id);
        }


        private static GameRatingBuilder BuildGameRatingBuilder(IRatingRepository ratingRepository = null, IBuilder<Rating, RatingViewModel> ratingBuilder = null, IGameRepository gameRepository = null) {
            ratingRepository = ratingRepository ?? new MockRatingRepository();
            ratingBuilder = ratingBuilder ?? new MockBuilder<Rating, RatingViewModel>();
            gameRepository = gameRepository ?? new MockGameRepository();
            return new GameRatingBuilder(ratingRepository, ratingBuilder, gameRepository);
        }
    }
}

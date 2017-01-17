using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.RatingContexts {
    public class RatingViewContextTest {
        [Fact]
        public void Initialize_Rating_View_Context() {
            var context = BuildRatingViewContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var ratingRepository = new MockRatingRepository().GetByStubbedToReturn(null);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>();
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildRatingViewContext(ratingRepository, ratingBuilder, gameBuilder);

            var ratingViewViewModel = await context.BuildViewModel(1000);

            ratingViewViewModel.Should().BeOfType<RatingViewViewModel>();
            ratingRepository.VerifyGetByCalled(1000);
            ratingBuilder.VerifyBuildNotCalled();
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_No_Associated_Games() {
            var rating = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "Rating",
                Description = "Description"
            };
            var ratingViewModel = new RatingViewModel {
                Id = rating.Id,
                Rate = rating.Rate,
                ShortDescription = rating.ShortDescription,
                Description = rating.Description,
                LongDescription = "1 - Rating - Description"
            };
            var ratingRepository = new MockRatingRepository().GetByIdStubbedToReturn(rating);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildRatingViewContext(ratingRepository, ratingBuilder, gameBuilder);

            var ratingViewViewModel = await context.BuildViewModel(rating.Id);

            ratingViewViewModel.Should().BeOfType<RatingViewViewModel>();
            ratingViewViewModel.Rating.Should().Be(ratingViewModel);
            ratingViewViewModel.Games.Should().BeEmpty();
            ratingRepository.VerifyGetByCalled(rating.Id);
            ratingBuilder.VerifyBuildCalled(rating);
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Returns_View_Model_With_Games() {
            var game = new Game {
                Id = 1,
                Name = "Game",
            };
            var rating = new Rating {
                Id = 1,
                Rate = 1,
                ShortDescription = "Rating",
                Description = "Description"
            };
            rating.GamePlayerRatings = new List<GamePlayerRating> {
                new GamePlayerRating {
                    Id = 1,
                    GameId = game.Id,
                    Game = game,
                    RatingId = rating.Id,
                    Rating= rating
                }
            };
            var ratingViewModel = new RatingViewModel {
                Id = rating.Id,
                Rate = rating.Rate,
                ShortDescription = rating.ShortDescription,
                Description = rating.Description,
                LongDescription = "1 - Rating - Description"
            }; 
             var gameViewModel = new GameViewModel {
                Id = game.Id,
                Name = game.Name
            };
            var ratingRepository = new MockRatingRepository().GetByIdStubbedToReturn(rating);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var gameBuilder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var context = BuildRatingViewContext(ratingRepository, ratingBuilder, gameBuilder);

            var ratingViewViewModel = await context.BuildViewModel(rating.Id);

            ratingViewViewModel.Should().BeOfType<RatingViewViewModel>();
            ratingViewViewModel.Rating.Should().Be(ratingViewModel);
            ratingViewViewModel.Games.Count().Should().Be(1);
            ratingViewViewModel.Games.Should().Contain(gameViewModel);
            ratingRepository.VerifyGetByCalled(rating.Id);
            ratingBuilder.VerifyBuildCalled(rating);
            gameBuilder.VerifyBuildCalled(game);
        }

        private static RatingViewContext BuildRatingViewContext(IRatingRepository ratingRepository = null, IBuilder<Rating, RatingViewModel> ratingBuilder = null, IBuilder<Game, GameViewModel> gameBuilder = null) {
            ratingRepository = ratingRepository ?? new MockRatingRepository();
            ratingBuilder = ratingBuilder ?? new MockBuilder<Rating, RatingViewModel>();
            gameBuilder = gameBuilder ?? new MockBuilder<Game, GameViewModel>();
            return new RatingViewContext(ratingRepository, ratingBuilder, gameBuilder);
        }
    }
}

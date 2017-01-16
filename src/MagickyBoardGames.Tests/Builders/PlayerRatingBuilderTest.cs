using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Builders
{
    public class PlayerRatingBuilderTest
    {
        [Fact]
        public void GamePlayerRating_To_View_Model() {
            var rating = new Rating {
                Id = 3,
                ShortDescription = "Short"
            };
            var ratingViewModel = new RatingViewModel {
                Description = "Description"
            };
            var player = new ApplicationUser {
                Id = "7",
                UserName = "User"
            };
            var gamePlayerRating = new GamePlayerRating {
                Id = 4,
                GameId = 2, 
                PlayerId = player.Id,
                RatingId = rating.Id
            };
            var ratingRepository = new MockRatingRepository().GetByIdStubbedToReturn(rating);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(ratingViewModel);
            var userRepository = new MockUserRepository().GetByIdStubbedToReturn(player);
            var builder = new PlayerRatingBuilder(ratingRepository, ratingBuilder, userRepository);

            var viewModel = builder.Build(gamePlayerRating);

            viewModel.PlayerName.Should().Be(player.UserName);
            viewModel.Rating.Should().Be(ratingViewModel);
            ratingRepository.VerifyGetByCalled(rating.Id);
            ratingBuilder.VerifyBuildCalled(rating);
            userRepository.VerifyGetByIdCalled(player.Id);
        }
    }
}

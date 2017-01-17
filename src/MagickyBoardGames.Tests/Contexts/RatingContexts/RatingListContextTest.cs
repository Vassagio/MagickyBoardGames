using System.Collections.Generic;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.RatingContexts
{
    public class RatingListContextTest
    {
        [Fact]
        public void Initializes() {
            var context = BuildRatingListContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var ratingRepository = new MockRatingRepository().GetByStubbedToReturn(null);
            var ratingBuilder = new MockBuilder<Rating, RatingViewModel>();
            var context = BuildRatingListContext(ratingRepository, ratingBuilder);

            var ratingListViewModel = await context.BuildViewModel();

            ratingListViewModel.Should().BeOfType<RatingListViewModel>();
            ratingRepository.VerifyGetAllCalled();
            ratingBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Build_View_Model() {
            var entity = new Rating();
            var entities = new List<Rating> {entity};
            var viewModel = new RatingViewModel();
            var repository = new MockRatingRepository().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<Rating, RatingViewModel>().BuildStubbedToReturn(viewModel);
            var context = BuildRatingListContext(repository, builder);

            var ratingListViewModel = await context.BuildViewModel();

            ratingListViewModel.Should().BeOfType<RatingListViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        private static RatingListContext BuildRatingListContext(IRatingRepository repository = null, IBuilder<Rating, RatingViewModel> builder = null) {
            repository = repository ?? new MockRatingRepository();
            builder = builder ?? new MockBuilder<Rating, RatingViewModel>();
            return new RatingListContext(repository, builder);
        }
    }
}

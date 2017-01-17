using FluentAssertions;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts.Rating;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MagickyBoardGames.Tests.Controllers {
    public class RatingControllerTest {
        [Fact]
        public void Initialize_A_New_Controller() {
            var controller = BuildRatingController();

            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public async void Display_Index_Result() {
            var viewModel = new RatingListViewModel();
            var context = new MockRatingListContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockRatingContextLoader().LoadRatingListContextStubbedToReturn(context);
            var controller = BuildRatingController(contextLoader);

            var result = await controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<RatingListViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled();
            contextLoader.VerifyLoadRatingListContextCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockRatingContextLoader();
            var controller = BuildRatingController(contextLoader);

            var result = await controller.Details(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadRatingViewContextNotCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
            var context = new MockRatingViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockRatingContextLoader().LoadRatingViewContextStubbedToReturn(context);
            ;
            var controller = BuildRatingController(contextLoader);

            var result = await controller.Details(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadRatingViewContextCalled();
        }

        [Fact]
        public async void Display_Details_Result() {
            var viewModel = new RatingViewViewModel();
            var context = new MockRatingViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockRatingContextLoader().LoadRatingViewContextStubbedToReturn(context);
            ;
            var controller = BuildRatingController(contextLoader);

            var result = await controller.Details(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<RatingViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadRatingViewContextCalled();
        }
       
        private static RatingController BuildRatingController(IRatingContextLoader contextLoader = null) {
            contextLoader = contextLoader ?? new MockRatingContextLoader();
            return new RatingController(contextLoader);
        }
    }
}
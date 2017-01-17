using FluentAssertions;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts.Player;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MagickyBoardGames.Tests.Controllers {
    public class PlayerControllerTest {
        [Fact]
        public void Initialize_A_New_Controller() {
            var controller = BuildPlayerController();

            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public async void Display_Index_Result() {
            var viewModel = new PlayerListViewModel();
            var context = new MockPlayerListContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockPlayerContextLoader().LoadPlayerListContextStubbedToReturn(context);
            var controller = BuildPlayerController(contextLoader);

            var result = await controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<PlayerListViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled();
            contextLoader.VerifyLoadPlayerListContextCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockPlayerContextLoader();
            var controller = BuildPlayerController(contextLoader);

            var result = await controller.Details(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadPlayerViewContextNotCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
            var context = new MockPlayerViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockPlayerContextLoader().LoadPlayerViewContextStubbedToReturn(context);
            ;
            var controller = BuildPlayerController(contextLoader);

            var result = await controller.Details("5");

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled("5");
            contextLoader.VerifyLoadPlayerViewContextCalled();
        }

        [Fact]
        public async void Display_Details_Result() {
            var viewModel = new PlayerViewViewModel();
            var context = new MockPlayerViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockPlayerContextLoader().LoadPlayerViewContextStubbedToReturn(context);
            ;
            var controller = BuildPlayerController(contextLoader);

            var result = await controller.Details("7");

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<PlayerViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled("7");
            contextLoader.VerifyLoadPlayerViewContextCalled();
        }
       
        private static PlayerController BuildPlayerController(IPlayerContextLoader contextLoader = null) {
            contextLoader = contextLoader ?? new MockPlayerContextLoader();
            return new PlayerController(contextLoader);
        }
    }
}
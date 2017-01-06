using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.Validations;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MagickyBoardGames.Tests.Controllers {
    public class GameControllerTest {
        //[Fact]
        //public void Initialize_A_New_Controller() {
        //    var controller = BuildGameController();

        //    controller.Should().NotBeNull();
        //    controller.Should().BeAssignableTo<Controller>();
        //}

        //[Fact]
        //public async void Display_Index_Result() {
        //    var viewModels = new List<GameViewModel> {
        //        new GameViewModel(),
        //        new GameViewModel()
        //    };
        //    var gameContext = new MockContext<GameViewModel>().GetAllStubbedToReturn(viewModels);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Index();

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<GameViewModel>>().Subject;
        //    model.Count().Should().Be(2);
        //    gameContext.VerifyGetAllCalled();
        //}

        //[Fact]
        //public async void Display_Details_Not_Found_When_Id_Is_Null() {
        //    var gameContext = new MockContext<GameViewModel>();
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Details(null);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByNotCalled();
        //}

        //[Fact]
        //public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(null);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Details(5);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByCalled(5);
        //}

        //[Fact]
        //public async void Display_Details_Result() {
        //    var foundViewModel = new GameViewModel {
        //        Id = 7,
        //        Description = "Found Item"
        //    };
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(foundViewModel);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Details(7);

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<GameViewModel>().Subject;
        //    model.Id.Should().Be(7);
        //    model.Description.Should().Be("Found Item");
        //    gameContext.VerifyGetByCalled(7);
        //}

        //[Fact]
        //public void Display_Create_Result() {
        //    var controller = BuildGameController();

        //    var result = controller.Create();

        //    result.Should().BeOfType<ViewResult>();
        //}

        //[Fact]
        //public async void Display_Create_Post_Result_Invalid() {
        //    var viewModel = new GameViewModel {
        //        Id = 9,
        //        Description = "Another Item"
        //    };
        //    var gameContext = new MockContext<GameViewModel>();
        //    var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnInvalid();
        //    var controller = BuildGameController(gameContext, validator);

        //    var result = await controller.Create(viewModel);

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<GameViewModel>().Subject;
        //    model.Id.Should().Be(9);
        //    model.Description.Should().Be("Another Item");
        //    gameContext.VerifyAddNotCalled();
        //    validator.VerifyValidateCalled(viewModel);
        //}

        //[Fact]
        //public async void Display_Create_Post_Result_Valid() {
        //    var viewModel = new GameViewModel {
        //        Id = 9,
        //        Description = "Another Item"
        //    };
        //    var gameContext = new MockContext<GameViewModel>();
        //    var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnValid();
        //    var controller = BuildGameController(gameContext, validator);

        //    var result = await controller.Create(viewModel);

        //    var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        //    var model = viewResult.ActionName.Should().Be("Index");
        //    gameContext.VerifyAddCalled(viewModel);
        //    validator.VerifyValidateCalled(viewModel);
        //}

        //[Fact]
        //public async void Display_Delete_Not_Found_When_Id_Is_Null() {
        //    var gameContext = new MockContext<GameViewModel>();
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Delete(null);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByNotCalled();
        //}

        //[Fact]
        //public async void Display_Delete_Not_Found_When_No_Record_Is_Found() {
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(null);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Delete(5);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByCalled(5);
        //}

        //[Fact]
        //public async void Display_Delete_Result() {
        //    var foundViewModel = new GameViewModel {
        //        Id = 7,
        //        Description = "Found Item"
        //    };
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(foundViewModel);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Delete(7);

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<GameViewModel>().Subject;
        //    model.Id.Should().Be(7);
        //    model.Description.Should().Be("Found Item");
        //    gameContext.VerifyGetByCalled(7);
        //}

        //[Fact]
        //public async void Display_Edit_Not_Found_When_Id_Is_Null() {
        //    var gameContext = new MockContext<GameViewModel>();
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Edit(null);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByNotCalled();
        //}

        //[Fact]
        //public async void Display_Edit_Not_Found_When_No_Record_Is_Found() {
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(null);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Edit(5);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyGetByCalled(5);
        //}

        //[Fact]
        //public async void Display_Edit_Result() {
        //    var foundViewModel = new GameViewModel {
        //        Id = 7,
        //        Description = "Found Item"
        //    };
        //    var gameContext = new MockContext<GameViewModel>().GetByStubbedToReturn(foundViewModel);
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Edit(7);

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<GameViewModel>().Subject;
        //    model.Id.Should().Be(7);
        //    model.Description.Should().Be("Found Item");
        //    gameContext.VerifyGetByCalled(7);
        //}

        //[Fact]
        //public async void Display_Edit_Not_Found_When_Id_Not_Equal() {
        //    var viewModel = new GameViewModel {
        //        Id = 22
        //    };
        //    var gameContext = new MockContext<GameViewModel>();
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.Edit(11, viewModel);
        //    result.Should().BeOfType<NotFoundResult>();

        //    gameContext.VerifyUpdateNotCalled();
        //}

        //[Fact]
        //public async void Display_Edit_Post_Result_Invalid() {
        //    var viewModel = new GameViewModel {
        //        Id = 22,
        //        Description = "Description"
        //    };
        //    var gameContext = new MockContext<GameViewModel>();
        //    var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnInvalid();
        //    var controller = BuildGameController(gameContext, validator);

        //    var result = await controller.Edit(22, viewModel);

        //    var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        //    var model = viewResult.Model.Should().BeAssignableTo<GameViewModel>().Subject;
        //    model.Id.Should().Be(22);
        //    model.Description.Should().Be("Description");
        //    gameContext.VerifyUpdateNotCalled();
        //    validator.VerifyValidateCalled(viewModel);
        //}

        //[Fact]
        //public async void Display_Edit_Post_Result_Valid() {
        //    var viewModel = new GameViewModel {
        //        Id = 22,
        //        Description = "Description"
        //    };
        //    var gameContext = new MockContext<GameViewModel>();
        //    var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnValid();
        //    var controller = BuildGameController(gameContext, validator);

        //    var result = await controller.Edit(22, viewModel);

        //    var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        //    viewResult.ActionName.Should().Be("Index");
        //    gameContext.VerifyUpdateCalled(viewModel);
        //    validator.VerifyValidateCalled(viewModel);
        //}

        //[Fact]
        //public async void Display_DeleteConfirmed_Result() {
        //    var gameContext = new MockContext<GameViewModel>();
        //    var controller = BuildGameController(gameContext);

        //    var result = await controller.DeleteConfirmed(11);

        //    var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        //    redirectResult.ActionName.Should().Be("Index");
        //    gameContext.VerifyDeleteCalled(11);
        //}

        //private static GameController BuildGameController(IContext<GameViewModel> gameContext = null, IValidator<GameViewModel> validator = null) {
        //    gameContext = gameContext ?? new MockContext<GameViewModel>();
        //    validator = validator ?? new MockValidator<GameViewModel>();
        //    return new GameController(gameContext, validator);
        //}
    }
}
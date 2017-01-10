using FluentAssertions;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MagickyBoardGames.Tests.Controllers {
    public class GameControllerTest {
        [Fact]
        public void Initialize_A_New_Controller() {
            var controller = BuildGameController();

            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public async void Display_Index_Result() {
            var viewModel = new GameListViewModel();
            var context = new MockGameListContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameListContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<GameListViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled();
            contextLoader.VerifyLoadGameListContextCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockGameContextLoader();
            var controller = BuildGameController(contextLoader);

            var result = await controller.Details(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameViewContextNotCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
            var context = new MockGameViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockGameContextLoader().LoadGameViewContextStubbedToReturn(context);
            ;
            var controller = BuildGameController(contextLoader);

            var result = await controller.Details(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadGameViewContextCalled();
        }

        [Fact]
        public async void Display_Details_Result() {
            var viewModel = new GameViewViewModel();
            var context = new MockGameViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameViewContextStubbedToReturn(context);
            ;
            var controller = BuildGameController(contextLoader);

            var result = await controller.Details(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<GameViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadGameViewContextCalled();
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockGameContextLoader();
            var controller = BuildGameController(contextLoader);

            var result = await controller.Delete(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameViewContextNotCalled();
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_No_Record_Is_Found() {
            var context = new MockGameViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockGameContextLoader().LoadGameViewContextStubbedToReturn(context);
            ;
            var controller = BuildGameController(contextLoader);

            var result = await controller.Delete(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadGameViewContextCalled();
        }

        [Fact]
        public async void Display_Delete_Result() {
            var viewModel = new GameViewViewModel();
            var context = new MockGameViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameViewContextStubbedToReturn(context);
            ;
            var controller = BuildGameController(contextLoader);

            var result = await controller.Delete(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<GameViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadGameViewContextCalled();
        }

        [Fact]
        public async void Display_DeleteConfirmed_Result() {
            var context = new MockGameViewContext();
            var contextLoader = new MockGameContextLoader().LoadGameViewContextStubbedToReturn(context);
            
            var controller = BuildGameController(contextLoader);

            var result = await controller.DeleteConfirmed(11);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be("Index");
            contextLoader.VerifyLoadGameViewContextCalled();
            context.VerifyDeleteCalled(11);
        }

        [Fact]
        public async void Display_Create_Result() {
            var viewModel = new GameSaveViewModel();
            var context = new MockGameSaveContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Create();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<GameSaveViewModel>();
            contextLoader.VerifyLoadGameSaveContextCalled();
            context.VerifyBuildViewModelCalled();
        }

        [Fact]
        public async void Display_Create_Post_Result_Invalid() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 9,
                    Name = "Another Item"
                }
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeInvalid();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Game.Id.Should().Be(9);
            model.Game.Name.Should().Be("Another Item");
            contextLoader.VerifyLoadGameSaveContextCalled();
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
        }

        [Fact]
        public async void Display_Create_Post_Result_Valid() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 9,
                    Name = "Another Item"
                }
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeValid();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            contextLoader.VerifyLoadGameSaveContextCalled();
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveCalled(viewModel);
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Is_Null() {           
            var context = new MockGameSaveContext();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameSaveContextNotCalled();
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_No_Record_Is_Found() {
            var context = new MockGameSaveContext().BuildViewModelFromIdStubbedToReturn(null);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelWithIdCalled(5);
            contextLoader.VerifyLoadGameSaveContextCalled();
        }

        [Fact]
        public async void Display_Edit_Result() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 7,
                    Name = "Found Item"
                }
            };
            var context = new MockGameSaveContext().BuildViewModelFromIdStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Should().Be(viewModel);
            context.VerifyBuildViewModelWithIdCalled(7);
            contextLoader.VerifyLoadGameSaveContextCalled();
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Not_Equal() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Name = "Found Item"
                }
            };
            var context = new MockGameSaveContext();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(7, viewModel);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameSaveContextNotCalled();
        }

        [Fact]
        public async void Display_Edit_Post_Result_Invalid() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 22,
                    Name = "Name"
                }
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeInvalid();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Game.Id.Should().Be(22);
            model.Game.Name.Should().Be("Name");
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
            contextLoader.VerifyLoadGameSaveContextCalled();
        }

        [Fact]
        public async void Display_Edit_Post_Result_Valid() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 22,
                    Name = "Name"
                }
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeValid();
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveCalled(viewModel);
            contextLoader.VerifyLoadGameSaveContextCalled();
        }

        private static GameController BuildGameController(IGameContextLoader contextLoader = null) {
            contextLoader = contextLoader ?? new MockGameContextLoader();
            return new GameController(contextLoader);
        }
    }
}
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Services;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.Tests.Mocks.MockContexts.Game;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Http;
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
        public async void Display_Index_Post_Result() {
            var viewModel = new GameListViewModel();
            var context = new MockGameListContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameListContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Index(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<GameListViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(viewModel);
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
            const string USER_ID = "1";
            var viewModel = new GameSaveViewModel();
            var context = new MockGameSaveContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var applicationUserManager = new MockApplicationUserManager().GetUserIdStubbedToReturn(USER_ID);
            var claimsPrincipal = new ClaimsPrincipal();
            var controller = BuildGameController(contextLoader, applicationUserManager, claimsPrincipal);

            var result = await controller.Create(0);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<GameSaveViewModel>().Subject;
            model.UserId.Should().Be(USER_ID);
            contextLoader.VerifyLoadGameSaveContextCalled();
            context.VerifyBuildViewModelCalled(0);
            applicationUserManager.VerifyGetUserIdCalled(claimsPrincipal);
        }

        [Fact]
        public async void Display_Create_Post_Result_Invalid() {
            var categoryViewModels = new List<CategoryViewModel> {
                new CategoryViewModel()
            };
            var ownerViewModels = new List<OwnerViewModel> {
                new OwnerViewModel()
            };
            var ratingViewModels = new List<RatingViewModel> {
                new RatingViewModel()
            };
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 9,
                    Name = "Another Item"
                },
                AvailableCategories = categoryViewModels,
                AvailableOwners = ownerViewModels,
                AvailableRatings = ratingViewModels
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeInvalid().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Game.Id.Should().Be(9);
            model.Game.Name.Should().Be("Another Item");
            model.AvailableCategories.ShouldBeEquivalentTo(categoryViewModels);
            model.AvailableOwners.ShouldBeEquivalentTo(ownerViewModels);
            model.AvailableRatings.ShouldBeEquivalentTo(ratingViewModels);
            contextLoader.VerifyLoadGameSaveContextCalled();
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
            context.VerifyBuildViewModelCalled();
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
            const string USER_ID = "1";
            var context = new MockGameSaveContext().BuildViewModelFromIdStubbedToReturn(null);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var applicationUserManager = new MockApplicationUserManager().GetUserIdStubbedToReturn(USER_ID);
            var controller = BuildGameController(contextLoader, applicationUserManager);

            var result = await controller.Edit(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5, USER_ID);
            contextLoader.VerifyLoadGameSaveContextCalled();
        }

        [Fact]
        public async void Display_Edit_Result() {
            const string USER_ID = "1";
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 7,
                    Name = "Found Item"
                }
            };
            var context = new MockGameSaveContext().BuildViewModelFromIdStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var applicationUserManager = new MockApplicationUserManager().GetUserIdStubbedToReturn(USER_ID);
            var controller = BuildGameController(contextLoader, applicationUserManager);

            var result = await controller.Edit(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7, USER_ID);
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
            var categoryViewModels = new List<CategoryViewModel> {
                new CategoryViewModel()
            };
            var ownerViewModels = new List<OwnerViewModel> {
                new OwnerViewModel()
            };
            var ratingViewModels = new List<RatingViewModel> {
                new RatingViewModel()
            };
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Id = 9,
                    Name = "Name"
                },
                AvailableCategories = categoryViewModels,
                AvailableOwners = ownerViewModels,
                AvailableRatings = ratingViewModels
            };
            var context = new MockGameSaveContext().ValidateStubbedToBeInvalid().BuildViewModelStubbedToReturn(viewModel);
            ;
            var contextLoader = new MockGameContextLoader().LoadGameSaveContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Edit(9, viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameSaveViewModel>().Subject;
            model.Game.Id.Should().Be(9);
            model.Game.Name.Should().Be("Name");
            model.AvailableCategories.ShouldBeEquivalentTo(categoryViewModels);
            model.AvailableOwners.ShouldBeEquivalentTo(ownerViewModels);
            model.AvailableRatings.ShouldBeEquivalentTo(ratingViewModels);
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
            context.VerifyBuildViewModelCalled();
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

        [Fact]
        public async void Display_Rate_Not_Found_When_Id_Is_Null() {
            var context = new MockGameRateContext();
            var contextLoader = new MockGameContextLoader().LoadGameRateContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Rate(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameRateContextNotCalled();
        }

        [Fact]
        public async void Display_Rate_Not_Found_When_No_Record_Is_Found() {
            const string USER_ID = "1";
            var context = new MockGameRateContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockGameContextLoader().LoadGameRateContextStubbedToReturn(context);
            var applicationUserManager = new MockApplicationUserManager().GetUserIdStubbedToReturn(USER_ID);
            var controller = BuildGameController(contextLoader, applicationUserManager);

            var result = await controller.Rate(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5, USER_ID);
            contextLoader.VerifyLoadGameRateContextCalled();
        }

        [Fact]
        public async void Display_Rate_Result() {
            const string USER_ID = "1";
            var viewModel = new GameRateViewModel {
                Game = new GameViewModel {
                    Id = 7,
                    Name = "Found Item"
                }
            };
            var context = new MockGameRateContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockGameContextLoader().LoadGameRateContextStubbedToReturn(context);
            var applicationUserManager = new MockApplicationUserManager().GetUserIdStubbedToReturn(USER_ID);
            var controller = BuildGameController(contextLoader, applicationUserManager);

            var result = await controller.Rate(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<GameRateViewModel>().Subject;
            model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7, USER_ID);
            contextLoader.VerifyLoadGameRateContextCalled();
        }

        [Fact]
        public async void Display_Rate_Not_Found_When_Id_Not_Equal() {
            var viewModel = new GameRateViewModel {
                Game = new GameViewModel {
                    Name = "Found Item"
                }
            };
            var context = new MockGameRateContext();
            var contextLoader = new MockGameContextLoader().LoadGameRateContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Rate(7, viewModel);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadGameRateContextNotCalled();
        }

        [Fact]
        public async void Display_Rate_Post_Result_Valid() {
            var viewModel = new GameRateViewModel {
                Game = new GameViewModel {
                    Id = 22,
                    Name = "Name"
                }
            };
            var context = new MockGameRateContext();
            var contextLoader = new MockGameContextLoader().LoadGameRateContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Rate(22, viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            context.VerifySaveCalled(viewModel);
            contextLoader.VerifyLoadGameRateContextCalled();
        }

        [Fact]
        public void Display_Search_Result() {
            var controller = BuildGameController();

            var result = controller.Search();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<ImportSearchViewModel>();
        }

        [Fact]
        public async void Display_Create_Post_Result() {
            var viewModel = new ImportSearchViewModel {
                Query = "Queyr"
            };
            var returnViewModel = new ImportSearchViewModel() {
                Query = "Query",
                BoardGames = new List<GameViewModel> {
                    new GameViewModel {
                        Name = "Game"
                    }
                }
            };
            var context = new MockGameSearchContext().BuildViewModelStubbedToReturn(returnViewModel);
            var contextLoader = new MockGameContextLoader().LoadGameSearchContextStubbedToReturn(context);
            var controller = BuildGameController(contextLoader);

            var result = await controller.Search(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<ImportSearchViewModel>().Subject;
            model.Should().Be(returnViewModel);
            contextLoader.VerifyLoadGameSearchContextCalled();
            context.VerifyBuildViewModelCalled(viewModel);
        }

        private static GameController BuildGameController(IGameContextLoader contextLoader = null, IApplicationUserManager applicationUserManager = null, ClaimsPrincipal claimsPrincipal = null) {
            contextLoader = contextLoader ?? new MockGameContextLoader();
            applicationUserManager = applicationUserManager ?? new MockApplicationUserManager();
            claimsPrincipal = claimsPrincipal ?? new ClaimsPrincipal();
            var controller = new GameController(contextLoader, applicationUserManager) {
                ControllerContext = new ControllerContext {
                    HttpContext = new DefaultHttpContext {
                        User = claimsPrincipal
                    }
                }
            };

            return controller;
        }
    }
}
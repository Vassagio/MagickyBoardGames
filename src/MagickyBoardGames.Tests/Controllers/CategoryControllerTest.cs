﻿using FluentAssertions;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Controllers;
using MagickyBoardGames.Tests.Mocks.MockContexts.Category;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MagickyBoardGames.Tests.Controllers {
    public class CategoryControllerTest {
        [Fact]
        public void Initialize_A_New_Controller() {
            var controller = BuildCategoryController();

            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public async void Display_Index_Result() {
            var viewModel = new CategoryListViewModel();
            var context = new MockCategoryListContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryListContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<CategoryListViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled();
            contextLoader.VerifyLoadCategoryListContextCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockCategoryContextLoader();
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Details(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadCategoryViewContextNotCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            ;
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Details(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_Details_Result() {
            var viewModel = new CategoryViewViewModel();
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            ;
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Details(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<CategoryViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_Id_Is_Null() {
            var contextLoader = new MockCategoryContextLoader();
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Delete(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadCategoryViewContextNotCalled();
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_No_Record_Is_Found() {
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            ;
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Delete(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_Delete_Result() {
            var viewModel = new CategoryViewViewModel();
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            ;
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Delete(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeAssignableTo<CategoryViewViewModel>();
            viewResult.Model.Should().Be(viewModel);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_DeleteConfirmed_Result() {
            var context = new MockCategoryViewContext();
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            ;
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.DeleteConfirmed(11);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be("Index");
            context.VerifyDeleteCalled(11);
        }

        [Fact]
        public void Display_Create_Result() {
            var controller = BuildCategoryController();

            var result = controller.Create();

            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async void Display_Create_Post_Result_Invalid() {
            var viewModel = new CategoryViewModel {
                Id = 9,
                Description = "Another Item"
            };
            var context = new MockCategorySaveContext().ValidateStubbedToBeInvalid();
            var contextLoader = new MockCategoryContextLoader().LoadCategorySaveContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(9);
            model.Description.Should().Be("Another Item");
            contextLoader.VerifyLoadCategorySaveContextCalled();
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
        }

        [Fact]
        public async void Display_Create_Post_Result_Valid() {
            var viewModel = new CategoryViewModel {
                Id = 9,
                Description = "Another Item"
            };
            var context = new MockCategorySaveContext().ValidateStubbedToBeValid();
            var contextLoader = new MockCategoryContextLoader().LoadCategorySaveContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            contextLoader.VerifyLoadCategorySaveContextCalled();
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveCalled(viewModel);
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Is_Null() {
            var context = new MockCategoryViewContext();
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(null);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadCategoryViewContextNotCalled();
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_No_Record_Is_Found() {
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(null);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(5);

            result.Should().BeOfType<NotFoundResult>();
            context.VerifyBuildViewModelCalled(5);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_Edit_Result() {
            var viewModel = new CategoryViewViewModel {
                Category = new CategoryViewModel {
                    Id = 7,
                    Description = "Found Item"
                }
            };
            var context = new MockCategoryViewContext().BuildViewModelStubbedToReturn(viewModel);
            var contextLoader = new MockCategoryContextLoader().LoadCategoryViewContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Should().Be(viewModel.Category);
            context.VerifyBuildViewModelCalled(7);
            contextLoader.VerifyLoadCategoryViewContextCalled();
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Not_Equal() {
            var viewModel = new CategoryViewModel {
                Description = "Found Item"
            };
            var context = new MockCategorySaveContext();
            var contextLoader = new MockCategoryContextLoader().LoadCategorySaveContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(7, viewModel);

            result.Should().BeOfType<NotFoundResult>();
            contextLoader.VerifyLoadCategorySaveContextNotCalled();
        }

        [Fact]
        public async void Display_Edit_Post_Result_Invalid() {
            var viewModel = new CategoryViewModel {
                Id = 22,
                Description = "Description"
            };
            var context = new MockCategorySaveContext().ValidateStubbedToBeInvalid();
            var contextLoader = new MockCategoryContextLoader().LoadCategorySaveContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(22);
            model.Description.Should().Be("Description");
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveNotCalled();
            contextLoader.VerifyLoadCategorySaveContextCalled();
        }

        [Fact]
        public async void Display_Edit_Post_Result_Valid() {
            var viewModel = new CategoryViewModel {
                Id = 22,
                Description = "Description"
            };
            var context = new MockCategorySaveContext().ValidateStubbedToBeValid();
            var contextLoader = new MockCategoryContextLoader().LoadCategorySaveContextStubbedToReturn(context);
            var controller = BuildCategoryController(contextLoader);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            context.VerifyValidateCalled(viewModel);
            context.VerifySaveCalled(viewModel);
            contextLoader.VerifyLoadCategorySaveContextCalled();
        }

        private static CategoryController BuildCategoryController(ICategoryContextLoader contextLoader = null) {
            contextLoader = contextLoader ?? new MockCategoryContextLoader();
            return new CategoryController(contextLoader);
        }
    }
}
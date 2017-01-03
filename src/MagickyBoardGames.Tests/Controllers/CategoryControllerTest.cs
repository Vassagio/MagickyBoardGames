﻿using System.Collections.Generic;
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
    public class CategoryControllerTest {
        [Fact]
        public void Initialize_A_New_Controller() {
            var controller = BuildCategoryController();

            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public async void Display_Index_Result() {
            var viewModels = new List<CategoryViewModel> {
                new CategoryViewModel(),
                new CategoryViewModel()
            };
            var categoryContext = new MockContext<CategoryViewModel>().GetAllStubbedToReturn(viewModels);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<CategoryViewModel>>().Subject;
            model.Count().Should().Be(2);
            categoryContext.VerifyGetAllCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_Id_Is_Null() {
            var categoryContext = new MockContext<CategoryViewModel>();
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Details(null);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByNotCalled();
        }

        [Fact]
        public async void Display_Details_Not_Found_When_No_Record_Is_Found() {
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(null);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Details(5);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByCalled(5);
        }

        [Fact]
        public async void Display_Details_Result() {
            var foundViewModel = new CategoryViewModel {
                Id = 7,
                Description = "Found Item"
            };
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(foundViewModel);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Details(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(7);
            model.Description.Should().Be("Found Item");
            categoryContext.VerifyGetByCalled(7);
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
            var categoryContext = new MockContext<CategoryViewModel>();
            var validator = new MockValidator<CategoryViewModel>().ValidateStubbedToReturnInvalid();
            var controller = BuildCategoryController(categoryContext, validator);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(9);
            model.Description.Should().Be("Another Item");
            categoryContext.VerifyAddNotCalled();
            validator.VerifyValidateCalled(viewModel);
        }

        [Fact]
        public async void Display_Create_Post_Result_Valid() {
            var viewModel = new CategoryViewModel {
                Id = 9,
                Description = "Another Item"
            };
            var categoryContext = new MockContext<CategoryViewModel>();
            var validator = new MockValidator<CategoryViewModel>().ValidateStubbedToReturnValid();
            var controller = BuildCategoryController(categoryContext, validator);

            var result = await controller.Create(viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            var model = viewResult.ActionName.Should().Be("Index");
            categoryContext.VerifyAddCalled(viewModel);
            validator.VerifyValidateCalled(viewModel);
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_Id_Is_Null() {
            var categoryContext = new MockContext<CategoryViewModel>();
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Delete(null);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByNotCalled();
        }

        [Fact]
        public async void Display_Delete_Not_Found_When_No_Record_Is_Found() {
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(null);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Delete(5);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByCalled(5);
        }

        [Fact]
        public async void Display_Delete_Result() {
            var foundViewModel = new CategoryViewModel {
                Id = 7,
                Description = "Found Item"
            };
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(foundViewModel);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Delete(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(7);
            model.Description.Should().Be("Found Item");
            categoryContext.VerifyGetByCalled(7);
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Is_Null() {
            var categoryContext = new MockContext<CategoryViewModel>();
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Edit(null);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByNotCalled();
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_No_Record_Is_Found() {
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(null);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Edit(5);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyGetByCalled(5);
        }

        [Fact]
        public async void Display_Edit_Result() {
            var foundViewModel = new CategoryViewModel {
                Id = 7,
                Description = "Found Item"
            };
            var categoryContext = new MockContext<CategoryViewModel>().GetByStubbedToReturn(foundViewModel);
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Edit(7);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(7);
            model.Description.Should().Be("Found Item");
            categoryContext.VerifyGetByCalled(7);
        }

        [Fact]
        public async void Display_Edit_Not_Found_When_Id_Not_Equal() {
            var viewModel = new CategoryViewModel {
                Id = 22
            };
            var categoryContext = new MockContext<CategoryViewModel>();
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.Edit(11, viewModel);
            result.Should().BeOfType<NotFoundResult>();

            categoryContext.VerifyUpdateNotCalled();
        }

        [Fact]
        public async void Display_Edit_Post_Result_Invalid() {
            var viewModel = new CategoryViewModel {
                Id = 22,
                Description = "Description"
            };
            var categoryContext = new MockContext<CategoryViewModel>();
            var validator = new MockValidator<CategoryViewModel>().ValidateStubbedToReturnInvalid();
            var controller = BuildCategoryController(categoryContext, validator);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<CategoryViewModel>().Subject;
            model.Id.Should().Be(22);
            model.Description.Should().Be("Description");
            categoryContext.VerifyUpdateNotCalled();
            validator.VerifyValidateCalled(viewModel);
        }

        [Fact]
        public async void Display_Edit_Post_Result_Valid() {
            var viewModel = new CategoryViewModel {
                Id = 22,
                Description = "Description"
            };
            var categoryContext = new MockContext<CategoryViewModel>();
            var validator = new MockValidator<CategoryViewModel>().ValidateStubbedToReturnValid();
            var controller = BuildCategoryController(categoryContext, validator);

            var result = await controller.Edit(22, viewModel);

            var viewResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            viewResult.ActionName.Should().Be("Index");
            categoryContext.VerifyUpdateCalled(viewModel);
            validator.VerifyValidateCalled(viewModel);
        }

        [Fact]
        public async void Display_DeleteConfirmed_Result() {
            var categoryContext = new MockContext<CategoryViewModel>();
            var controller = BuildCategoryController(categoryContext);

            var result = await controller.DeleteConfirmed(11);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be("Index");
            categoryContext.VerifyDeleteCalled(11);
        }

        private static CategoryController BuildCategoryController(IContext<CategoryViewModel> categoryContext = null, IValidator<CategoryViewModel> validator = null) {
            categoryContext = categoryContext ?? new MockContext<CategoryViewModel>();
            validator = validator ?? new MockValidator<CategoryViewModel>();
            return new CategoryController(categoryContext, validator);
        }
    }
}
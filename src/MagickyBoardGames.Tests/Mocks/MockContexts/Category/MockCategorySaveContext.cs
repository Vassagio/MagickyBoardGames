using System.Threading.Tasks;
using FluentValidation.Results;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Category {
    public class MockCategorySaveContext : ICategorySaveContext {
        private readonly Mock<ICategorySaveContext> _mock;

        public MockCategorySaveContext() {
            _mock = new Mock<ICategorySaveContext>();
        }

        public ValidationResult Validate(CategoryViewModel viewModel) {
            return _mock.Object.Validate(viewModel);
        }

        public Task Save(CategoryViewModel viewModel) {
            return _mock.Object.Save(viewModel);
        }

        public void VerifyValidateCalled(CategoryViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Validate(viewModel), Times.Exactly(times));
        }

        public void VerifySaveCalled(CategoryViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Save(viewModel), Times.Exactly(times));
        }

        public MockCategorySaveContext ValidateStubbedToBeValid() {
            var results = new ValidationResult();
            _mock.Setup(m => m.Validate(It.IsAny<CategoryViewModel>())).Returns(results);
            return this;
        }

        public MockCategorySaveContext ValidateStubbedToBeInvalid() {
            var results = new ValidationResult();
            results.Errors.Add(new ValidationFailure("Property", "Error"));
            _mock.Setup(m => m.Validate(It.IsAny<CategoryViewModel>())).Returns(results);
            return this;
        }

        public void VerifySaveNotCalled() {
            _mock.Verify(m => m.Save(It.IsAny<CategoryViewModel>()), Times.Never);
        }
    }
}
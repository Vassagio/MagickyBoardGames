using System.Threading.Tasks;
using FluentValidation.Results;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts {
    public class MockGameSaveContext : IGameSaveContext {
        private readonly Mock<IGameSaveContext> _mock;

        public MockGameSaveContext() {
            _mock = new Mock<IGameSaveContext>();
        }

        public ValidationResult Validate(GameSaveViewModel viewModel) {
            return _mock.Object.Validate(viewModel);
        }

        public Task Save(GameSaveViewModel viewModel) {
            return _mock.Object.Save(viewModel);
        }

        public Task<GameSaveViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public Task<GameSaveViewModel> BuildViewModel(int id) {
            return _mock.Object.BuildViewModel(id);
        }

        public void VerifyValidateCalled(GameSaveViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Validate(viewModel), Times.Exactly(times));
        }

        public void VerifySaveCalled(GameSaveViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Save(viewModel), Times.Exactly(times));
        }

        public MockGameSaveContext ValidateStubbedToBeValid() {
            var results = new ValidationResult();
            _mock.Setup(m => m.Validate(It.IsAny<GameSaveViewModel>())).Returns(results);
            return this;
        }

        public MockGameSaveContext ValidateStubbedToBeInvalid() {
            var results = new ValidationResult();
            results.Errors.Add(new ValidationFailure("Property", "Error"));
            _mock.Setup(m => m.Validate(It.IsAny<GameSaveViewModel>())).Returns(results);
            return this;
        }

        public void VerifySaveNotCalled() {
            _mock.Verify(m => m.Save(It.IsAny<GameSaveViewModel>()), Times.Never);
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }

        public void VerifyBuildViewModelWithIdCalled(int id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }

        public MockGameSaveContext BuildViewModelStubbedToReturn(GameSaveViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public MockGameSaveContext BuildViewModelFromIdStubbedToReturn(GameSaveViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }
    }
}
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
    }
}
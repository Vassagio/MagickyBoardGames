using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Game {
    public class MockGameSearchContext : IGameSearchContext {
        private readonly Mock<IGameSearchContext> _mock = new Mock<IGameSearchContext>();

        public Task<ImportSearchViewModel> BuildViewModel(ImportSearchViewModel viewModel) {
            return _mock.Object.BuildViewModel(viewModel);
        }

        public MockGameSearchContext BuildViewModelStubbedToReturn(ImportSearchViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<ImportSearchViewModel>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(ImportSearchViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(viewModel), Times.Exactly(times));
        }
    }
}
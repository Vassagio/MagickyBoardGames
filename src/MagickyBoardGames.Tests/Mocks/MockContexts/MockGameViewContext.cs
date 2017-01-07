using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockGameViewContext: IGameViewContext {
        private readonly Mock<IGameViewContext> _mock;

        public MockGameViewContext() {
            _mock = new Mock<IGameViewContext>();
        }

        public Task<GameViewViewModel> BuildViewModel(int id) {
            return _mock.Object.BuildViewModel(id);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public MockGameViewContext BuildViewModelStubbedToReturn(GameViewViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }

        public void VerifyDeleteCalled(int id, int times = 1) {
            _mock.Verify(m => m.Delete(id), Times.Exactly(times));
        }
    }
}

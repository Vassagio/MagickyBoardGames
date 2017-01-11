using System.Threading.Tasks;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockPlayerViewContext: IPlayerViewContext {
        private readonly Mock<IPlayerViewContext> _mock;

        public MockPlayerViewContext() {
            _mock = new Mock<IPlayerViewContext>();
        }

        public Task<PlayerViewViewModel> BuildViewModel(string id) {
            return _mock.Object.BuildViewModel(id);
        }

        public MockPlayerViewContext BuildViewModelStubbedToReturn(PlayerViewViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<string>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(string id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }
    }
}

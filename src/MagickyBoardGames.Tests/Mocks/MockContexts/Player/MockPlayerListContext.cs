using System.Threading.Tasks;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Player
{
    public class MockPlayerListContext: IPlayerListContext
    {
        private readonly Mock<IPlayerListContext> _mock;

        public MockPlayerListContext() {
            _mock = new Mock<IPlayerListContext>();
        }

        public Task<PlayerListViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public MockPlayerListContext BuildViewModelStubbedToReturn(PlayerListViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }
    }
}

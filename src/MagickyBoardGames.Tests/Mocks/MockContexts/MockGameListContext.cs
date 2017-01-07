using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockGameListContext: IGameListContext
    {
        private readonly Mock<IGameListContext> _mock;

        public MockGameListContext() {
            _mock = new Mock<IGameListContext>();
        }

        public Task<GameListViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public MockGameListContext BuildViewModelStubbedToReturn(GameListViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }
    }
}

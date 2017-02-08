using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Game
{
    public class MockGameListContext: IGameListContext
    {
        private readonly Mock<IGameListContext> _mock;

        public MockGameListContext() {
            _mock = new Mock<IGameListContext>();
        }

        public Task<GameListViewModel> BuildViewModel(GameListViewModel viewModel) {
            return _mock.Object.BuildViewModel(viewModel);
        }

        public MockGameListContext BuildViewModelStubbedToReturn(GameListViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny< GameListViewModel>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(GameListViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(viewModel), Times.Exactly(times));
        }
    }
}

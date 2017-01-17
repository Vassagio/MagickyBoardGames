using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Game {
    public class MockGameRateContext : IGameRateContext {
        private readonly Mock<IGameRateContext> _mock;

        public MockGameRateContext() {
            _mock = new Mock<IGameRateContext>();
        }
        public Task<GameRateViewModel> BuildViewModel(int gameId, string playerId) {
            return _mock.Object.BuildViewModel(gameId, playerId);
        }
        public Task Save(GameRateViewModel viewModel) {
            return _mock.Object.Save(viewModel);
        }

        public MockGameRateContext BuildViewModelStubbedToReturn(GameRateViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int gameId, string playerId, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(gameId, playerId), Times.Exactly(times));
        }

        public void VerifySaveCalled(GameRateViewModel viewModel, int times= 1) {
            _mock.Verify(m => m.Save(viewModel), Times.Exactly(times));
        }

    }
}
using MagickyBoardGames.Contexts.PlayerContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Player {
    public class MockPlayerContextLoader : IPlayerContextLoader {
        private readonly Mock<IPlayerContextLoader> _mock;

        public MockPlayerContextLoader() {
            _mock = new Mock<IPlayerContextLoader>();
        }

        public IPlayerListContext LoadPlayerListContext() {
            return _mock.Object.LoadPlayerListContext();
        }

        public IPlayerViewContext LoadPlayerViewContext() {
            return _mock.Object.LoadPlayerViewContext();
        }

        public MockPlayerContextLoader LoadPlayerListContextStubbedToReturn(IPlayerListContext context) {
            _mock.Setup(m => m.LoadPlayerListContext()).Returns(context);
            return this;
        }

        public void VerifyLoadPlayerListContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadPlayerListContext(), Times.Exactly(times));
        }

        public void VerifyLoadPlayerViewContextNotCalled() {
            _mock.Verify(m => m.LoadPlayerViewContext(), Times.Never);
        }

        public void VerifyLoadPlayerViewContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadPlayerViewContext(), Times.Exactly(times));
        }

        public MockPlayerContextLoader LoadPlayerViewContextStubbedToReturn(IPlayerViewContext context) {
            _mock.Setup(m => m.LoadPlayerViewContext()).Returns(context);
            return this;
        }
    }
}
using MagickyBoardGames.Contexts.GameContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts {
    public class MockGameContextLoader : IGameContextLoader {
        private readonly Mock<IGameContextLoader> _mock;

        public MockGameContextLoader() {
            _mock = new Mock<IGameContextLoader>();
        }

        public IGameListContext LoadGameListContext() {
            return _mock.Object.LoadGameListContext();
        }

        public IGameViewContext LoadGameViewContext() {
            return _mock.Object.LoadGameViewContext();
        }

        public IGameSaveContext LoadGameSaveContext() {
            return _mock.Object.LoadGameSaveContext();
        }

        public IGameRateContext LoadGameRateContext() {
            return _mock.Object.LoadGameRateContext();
        }

        public MockGameContextLoader LoadGameListContextStubbedToReturn(IGameListContext context) {
            _mock.Setup(m => m.LoadGameListContext()).Returns(context);
            return this;
        }

        public void VerifyLoadGameListContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadGameListContext(), Times.Exactly(times));
        }

        public void VerifyLoadGameViewContextNotCalled() {
            _mock.Verify(m => m.LoadGameViewContext(), Times.Never);
        }

        public void VerifyLoadGameViewContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadGameViewContext(), Times.Exactly(times));
        }

        public MockGameContextLoader LoadGameViewContextStubbedToReturn(IGameViewContext context) {
            _mock.Setup(m => m.LoadGameViewContext()).Returns(context);
            return this;
        }

        public MockGameContextLoader LoadGameSaveContextStubbedToReturn(IGameSaveContext context) {
            _mock.Setup(m => m.LoadGameSaveContext()).Returns(context);
            return this;
        }

        public MockGameContextLoader LoadGameRateContextStubbedToReturn(IGameRateContext context) {
            _mock.Setup(m => m.LoadGameRateContext()).Returns(context);
            return this;
        }

        public void VerifyLoadGameSaveContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadGameSaveContext(), Times.Exactly(times));
        }

        public void VerifyLoadGameSaveContextNotCalled() {
            _mock.Verify(m => m.LoadGameSaveContext(), Times.Never);
        }

        public void VerifyLoadGameRateContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadGameRateContext(), Times.Exactly(times));
        }

        public void VerifyLoadGameRateContextNotCalled() {
            _mock.Verify(m => m.LoadGameRateContext(), Times.Never);
        }
    }
}
using MagickyBoardGames.Contexts.RatingContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Rating {
    public class MockRatingContextLoader : IRatingContextLoader {
        private readonly Mock<IRatingContextLoader> _mock;

        public MockRatingContextLoader() {
            _mock = new Mock<IRatingContextLoader>();
        }

        public IRatingListContext LoadRatingListContext() {
            return _mock.Object.LoadRatingListContext();
        }

        public IRatingViewContext LoadRatingViewContext() {
            return _mock.Object.LoadRatingViewContext();
        }

        public MockRatingContextLoader LoadRatingListContextStubbedToReturn(IRatingListContext context) {
            _mock.Setup(m => m.LoadRatingListContext()).Returns(context);
            return this;
        }

        public void VerifyLoadRatingListContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadRatingListContext(), Times.Exactly(times));
        }

        public void VerifyLoadRatingViewContextNotCalled() {
            _mock.Verify(m => m.LoadRatingViewContext(), Times.Never);
        }

        public void VerifyLoadRatingViewContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadRatingViewContext(), Times.Exactly(times));
        }

        public MockRatingContextLoader LoadRatingViewContextStubbedToReturn(IRatingViewContext context) {
            _mock.Setup(m => m.LoadRatingViewContext()).Returns(context);
            return this;
        }
    }
}
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.CategoryContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts {
    public class MockContextLoader : IContextLoader {
        private readonly Mock<IContextLoader> _mock;

        public MockContextLoader() {
            _mock = new Mock<IContextLoader>();
        }

        public ICategoryIndexContext LoadCategoryIndexContext() {
            return _mock.Object.LoadCategoryIndexContext();
        }

        public ICategoryDetailContext LoadCategoryDetailContext() {
            return _mock.Object.LoadCategoryDetailContext();
        }

        public MockContextLoader LoadCategoryIndexContextStubbedToReturn(ICategoryIndexContext context) {
            _mock.Setup(m => m.LoadCategoryIndexContext()).Returns(context);
            return this;
        }

        public void VerifyLoadCategoryIndexContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategoryIndexContext(), Times.Exactly(times));
        }

        public void VerifyLoadCategoryDetailContextNotCalled() {
            _mock.Verify(m => m.LoadCategoryDetailContext(), Times.Never);
        }

        public void VerifyLoadCategoryDetailContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategoryDetailContext(), Times.Exactly(times));
        }

        public MockContextLoader LoadCategoryDetailContextStubbedToReturn(ICategoryDetailContext context) {
            _mock.Setup(m => m.LoadCategoryDetailContext()).Returns(context);
            return this;
        }
    }
}
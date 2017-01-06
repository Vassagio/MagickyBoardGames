using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.CategoryContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts {
    public class MockContextLoader : IContextLoader {
        private readonly Mock<IContextLoader> _mock;

        public MockContextLoader() {
            _mock = new Mock<IContextLoader>();
        }

        public ICategoryListContext LoadCategoryListContext() {
            return _mock.Object.LoadCategoryListContext();
        }

        public ICategoryViewContext LoadCategoryViewContext() {
            return _mock.Object.LoadCategoryViewContext();
        }

        public ICategorySaveContext LoadCategorySaveContext() {
            return _mock.Object.LoadCategorySaveContext();
        }

        public MockContextLoader LoadCategoryListContextStubbedToReturn(ICategoryListContext context) {
            _mock.Setup(m => m.LoadCategoryListContext()).Returns(context);
            return this;
        }

        public void VerifyLoadCategoryListContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategoryListContext(), Times.Exactly(times));
        }

        public void VerifyLoadCategoryViewContextNotCalled() {
            _mock.Verify(m => m.LoadCategoryViewContext(), Times.Never);
        }

        public void VerifyLoadCategoryViewContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategoryViewContext(), Times.Exactly(times));
        }

        public MockContextLoader LoadCategoryViewContextStubbedToReturn(ICategoryViewContext context) {
            _mock.Setup(m => m.LoadCategoryViewContext()).Returns(context);
            return this;
        }

        public MockContextLoader LoadCategorySaveContextStubbedToReturn(ICategorySaveContext context) {
            _mock.Setup(m => m.LoadCategorySaveContext()).Returns(context);
            return this;
        }

        public void VerifyLoadCategorySaveContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategorySaveContext(), Times.Exactly(times));
        }
    }
}
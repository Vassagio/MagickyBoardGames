using MagickyBoardGames.Contexts.CategoryContexts;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Category {
    public class MockCategoryContextLoader : ICategoryContextLoader {
        private readonly Mock<ICategoryContextLoader> _mock;

        public MockCategoryContextLoader() {
            _mock = new Mock<ICategoryContextLoader>();
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

        public MockCategoryContextLoader LoadCategoryListContextStubbedToReturn(ICategoryListContext context) {
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

        public MockCategoryContextLoader LoadCategoryViewContextStubbedToReturn(ICategoryViewContext context) {
            _mock.Setup(m => m.LoadCategoryViewContext()).Returns(context);
            return this;
        }

        public MockCategoryContextLoader LoadCategorySaveContextStubbedToReturn(ICategorySaveContext context) {
            _mock.Setup(m => m.LoadCategorySaveContext()).Returns(context);
            return this;
        }

        public void VerifyLoadCategorySaveContextCalled(int times = 1) {
            _mock.Verify(m => m.LoadCategorySaveContext(), Times.Exactly(times));
        }

        public void VerifyLoadCategorySaveContextNotCalled() {
            _mock.Verify(m => m.LoadCategorySaveContext(), Times.Never);
        }
    }
}
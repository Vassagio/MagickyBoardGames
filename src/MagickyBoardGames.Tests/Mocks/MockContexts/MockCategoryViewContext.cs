using System.Threading.Tasks;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockCategoryViewContext: ICategoryViewContext {
        private readonly Mock<ICategoryViewContext> _mock;

        public MockCategoryViewContext() {
            _mock = new Mock<ICategoryViewContext>();
        }

        public Task<CategoryViewViewModel> BuildViewModel(int id) {
            return _mock.Object.BuildViewModel(id);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public MockCategoryViewContext BuildViewModelStubbedToReturn(CategoryViewViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }

        public void VerifyDeleteCalled(int id, int times = 1) {
            _mock.Verify(m => m.Delete(id), Times.Exactly(times));
        }
    }
}

using System.Threading.Tasks;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockCategoryDetailContext: ICategoryDetailContext {
        private readonly Mock<ICategoryDetailContext> _mock;

        public MockCategoryDetailContext() {
            _mock = new Mock<ICategoryDetailContext>();
        }

        public Task<CategoryDetailViewModel> BuildViewModel(int id) {
            return _mock.Object.BuildViewModel(id);
        }

        public MockCategoryDetailContext BuildViewModelStubbedToReturn(CategoryDetailViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }
    }
}

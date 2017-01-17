using System.Threading.Tasks;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Category
{
    public class MockCategoryListContext: ICategoryListContext
    {
        private readonly Mock<ICategoryListContext> _mock;

        public MockCategoryListContext() {
            _mock = new Mock<ICategoryListContext>();
        }

        public Task<CategoryListViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public MockCategoryListContext BuildViewModelStubbedToReturn(CategoryListViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }
    }
}

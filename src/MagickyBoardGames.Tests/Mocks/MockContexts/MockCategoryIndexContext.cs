using System.Threading.Tasks;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts
{
    public class MockCategoryIndexContext: ICategoryIndexContext
    {
        private readonly Mock<ICategoryIndexContext> _mock;

        public MockCategoryIndexContext() {
            _mock = new Mock<ICategoryIndexContext>();
        }

        public Task<CategoryIndexViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public MockCategoryIndexContext BuildViewModelStubbedToReturn(CategoryIndexViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }
    }
}

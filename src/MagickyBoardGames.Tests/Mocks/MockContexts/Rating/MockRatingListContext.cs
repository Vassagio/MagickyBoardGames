using System.Threading.Tasks;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Rating
{
    public class MockRatingListContext: IRatingListContext
    {
        private readonly Mock<IRatingListContext> _mock;

        public MockRatingListContext() {
            _mock = new Mock<IRatingListContext>();
        }

        public Task<RatingListViewModel> BuildViewModel() {
            return _mock.Object.BuildViewModel();
        }

        public MockRatingListContext BuildViewModelStubbedToReturn(RatingListViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel()).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int times = 1) {
            _mock.Verify(m => m.BuildViewModel(), Times.Exactly(times));
        }
    }
}

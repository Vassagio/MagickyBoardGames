using System.Threading.Tasks;
using MagickyBoardGames.Contexts.RatingContexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks.MockContexts.Rating
{
    public class MockRatingViewContext: IRatingViewContext {
        private readonly Mock<IRatingViewContext> _mock;

        public MockRatingViewContext() {
            _mock = new Mock<IRatingViewContext>();
        }

        public Task<RatingViewViewModel> BuildViewModel(int id) {
            return _mock.Object.BuildViewModel(id);
        }

        public MockRatingViewContext BuildViewModelStubbedToReturn(RatingViewViewModel viewModel) {
            _mock.Setup(m => m.BuildViewModel(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyBuildViewModelCalled(int id, int times = 1) {
            _mock.Verify(m => m.BuildViewModel(id), Times.Exactly(times));
        }
    }
}

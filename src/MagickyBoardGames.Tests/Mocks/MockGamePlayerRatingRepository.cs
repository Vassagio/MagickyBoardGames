using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockGamePlayerRatingRepository : IGamePlayerRatingRepository {
        private readonly Mock<IGamePlayerRatingRepository> _mock;

        public MockGamePlayerRatingRepository() {
            _mock = new Mock<IGamePlayerRatingRepository>();
        }

        public Task Save(int gameId, string playerId, int ratingId) {
            return _mock.Object.Save(gameId, playerId, ratingId);
        }
        public Task<GamePlayerRating> GetBy(int gameId, string playerId) {
            return _mock.Object.GetBy(gameId, playerId);
        }

        public MockGamePlayerRatingRepository GetByStubbedToReturn(GamePlayerRating gamePlayerRating) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(gamePlayerRating));
            return this;
        }

        public void VerifySaveCalled(int gameId, string playerId, int ratingId, int times = 1) {
            _mock.Verify(m => m.Save(gameId, playerId, ratingId), Times.Exactly(times));
        }

        public void VerifyGetByCalled(int gameId, string playerId, int times = 1) {
            _mock.Verify(m => m.GetBy(gameId, playerId), Times.Exactly(times));
        }

        public void VerifySaveNotCalled() {
            _mock.Verify(m => m.Save(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
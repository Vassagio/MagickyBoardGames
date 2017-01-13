using System.Threading.Tasks;
using MagickyBoardGames.Repositories;
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

        public void VerifySaveCalled(int gameId, string playerId, int ratingId, int times = 1) {
            _mock.Verify(m => m.Save(gameId, playerId, ratingId), Times.Exactly(times));
        }
    }
}
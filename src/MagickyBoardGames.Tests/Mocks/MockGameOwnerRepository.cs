using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockGameOwnerRepository : IGameOwnerRepository {
        private readonly Mock<IGameOwnerRepository> _mock;

        public MockGameOwnerRepository() {
            _mock = new Mock<IGameOwnerRepository>();
        }

        public Task Adjust(int id, IEnumerable<ApplicationUser> owners) {
            return _mock.Object.Adjust(id, owners);
        }

        public void VerifyAdjustCalled(int id, List<ApplicationUser> owners, int times = 1) {
            _mock.Verify(m => m.Adjust(id, owners), Times.Exactly(times));
        }
    }
}
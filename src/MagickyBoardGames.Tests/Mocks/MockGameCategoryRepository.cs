using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockGameCategoryRepository : IGameCategoryRepository {
        private readonly Mock<IGameCategoryRepository> _mock;

        public MockGameCategoryRepository() {
            _mock = new Mock<IGameCategoryRepository>();
        }

        public Task Adjust(int id, IEnumerable<Category> categories) {
            return _mock.Object.Adjust(id, categories);
        }

        public void VerifyAdjustCalled(int id, IEnumerable<Category> categories, int times = 1) {
            _mock.Verify(m => m.Adjust(id, categories), Times.Exactly(times));
        }
    }
}
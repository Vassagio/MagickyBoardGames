using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockRatingRepository : IRatingRepository {
        private readonly Mock<IRatingRepository> _mock;

        public MockRatingRepository() {
            _mock = new Mock<IRatingRepository>();
        }

        public Task<IEnumerable<Rating>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<Rating> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<Rating> GetBy(string name) {
            return _mock.Object.GetBy(name);
        }

        public MockRatingRepository GetAllStubbedToReturn(IEnumerable<Rating> owners) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(owners));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public MockRatingRepository GetByStubbedToReturn(Rating rating) {
            _mock.Setup(m => m.GetBy(It.IsAny<string>())).Returns(Task.FromResult(rating));
            return this;
        }

        public MockRatingRepository GetByIdStubbedToReturn(Rating rating) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(rating));
            return this;
        }

        public void VerifyGetByCalled(int id, int times = 1) {
            _mock.Verify(m => m.GetBy(id), Times.Exactly(times));
        }

        public void VerifyGetByCalled(string name, int times = 1) {
            _mock.Verify(m => m.GetBy(name), Times.Exactly(times));
        }
    }
}
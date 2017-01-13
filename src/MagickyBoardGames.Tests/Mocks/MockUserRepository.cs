using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockUserRepository : IUserRepository {
        private readonly Mock<IUserRepository> _mock;

        public MockUserRepository() {
            _mock = new Mock<IUserRepository>();
        }

        public Task<IEnumerable<ApplicationUser>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<ApplicationUser> GetById(string id) {
            return _mock.Object.GetById(id);
        }

        public Task<ApplicationUser> GetBy(string name) {
            return _mock.Object.GetBy(name);
        }

        public MockUserRepository GetAllStubbedToReturn(IEnumerable<ApplicationUser> owners) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(owners));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public MockUserRepository GetByStubbedToReturn(ApplicationUser user) {
            _mock.Setup(m => m.GetBy(It.IsAny<string>())).Returns(Task.FromResult(user));
            return this;
        }

        public MockUserRepository GetByIdStubbedToReturn(ApplicationUser user) {
            _mock.Setup(m => m.GetById(It.IsAny<string>())).Returns(Task.FromResult(user));
            return this;
        }

        public void VerifyGetByIdCalled(string id, int times = 1) {
            _mock.Verify(m => m.GetById(id), Times.Exactly(times));
        }

        public void VerifyGetByCalled(string name, int times = 1) {
            _mock.Verify(m => m.GetBy(name), Times.Exactly(times));
        }
    }
}
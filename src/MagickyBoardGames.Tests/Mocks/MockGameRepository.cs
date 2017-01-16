using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockGameRepository : IGameRepository {
        private readonly Mock<IGameRepository> _mock;

        public MockGameRepository() {
            _mock = new Mock<IGameRepository>();
        }

        public Task<Game> GetBy(string name) {
            return _mock.Object.GetBy(name);
        }

        public Task<IEnumerable<Game>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<Game> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<int> Add(Game game) {
            return _mock.Object.Add(game);
        }

        public Task<int> Add(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string userId) {
            return _mock.Object.Add(game, categories, owners, ratingId, userId);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public Task Update(Game game) {
            return _mock.Object.Update(game);
        }

        public Task Update(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string userId) {
            return _mock.Object.Update(game, categories, owners, ratingId, userId);
        }

        public MockGameRepository GetAllStubbedToReturn(IEnumerable<Game> entities) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(entities));
            return this;
        }

        public MockGameRepository GetByStubbedToReturn(Game game) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(game));
            _mock.Setup(m => m.GetBy(It.IsAny<string>())).Returns(Task.FromResult(game));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public void VerifyGetByCalled(int id, int times = 1) {
            _mock.Verify(m => m.GetBy(id), Times.Exactly(times));
        }

        public void VerifyGetByCalled(string name, int times = 1) {
            _mock.Verify(m => m.GetBy(name), Times.Exactly(times));
        }

        public void VerifyGetByIdNotCalled() {
            _mock.Verify(m => m.GetBy(It.IsAny<int>()), Times.Never);
        }

        public void VerifyGetByNameNotCalled() {
            _mock.Verify(m => m.GetBy(It.IsAny<string>()), Times.Never);
        }

        public void VerifyDeleteCalled(int id, int times = 1) {
            _mock.Verify(m => m.Delete(id), Times.Exactly(times));
        }

        public void VerifyAddCalled(Game game, int times = 1) {
            _mock.Verify(m => m.Add(game), Times.Exactly(times));
        }

        public void VerifyAddCalled(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string playerId, int times = 1) {
            _mock.Verify(m => m.Add(game, categories, owners, ratingId, playerId), Times.Exactly(times));
        }

        public void VerifyAddNotCalled() {
            _mock.Verify(m => m.Add(It.IsAny<Game>()), Times.Never);
        }

        public void VerifyUpdateCalled(Game game, int times = 1) {
            _mock.Verify(m => m.Update(game), Times.Exactly(times));
        }

        public void VerifyUpdateCalled(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string playerId, int times = 1) {
            _mock.Verify(m => m.Update(game, categories, owners, ratingId, playerId), Times.Exactly(times));
        }

        public void VerifyUpdateNotCalled() {
            _mock.Verify(m => m.Update(It.IsAny<Game>()), Times.Never);
        }
    }
}
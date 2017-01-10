using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockCategoryRepository : ICategoryRepository {
        private readonly Mock<ICategoryRepository> _mock;

        public MockCategoryRepository() {
            _mock = new Mock<ICategoryRepository>();
        }

        public Task<Category> GetBy(string description) {
            return _mock.Object.GetBy(description);
        }

        public Task<IEnumerable<Category>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<Category> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<int> Add(Category category) {
            return _mock.Object.Add(category);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public Task Update(Category category) {
            return _mock.Object.Update(category);
        }

        public MockCategoryRepository GetAllStubbedToReturn(IEnumerable<Category> entities) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(entities));
            return this;
        }

        public MockCategoryRepository GetByStubbedToReturn(Category category) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(category));
            _mock.Setup(m => m.GetBy(It.IsAny<string>())).Returns(Task.FromResult(category));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public void VerifyGetByCalled(int id, int times = 1) {
            _mock.Verify(m => m.GetBy(id), Times.Exactly(times));
        }

        public void VerifyGetByCalled(string description, int times = 1) {
            _mock.Verify(m => m.GetBy(description), Times.Exactly(times));
        }

        public void VerifyGetByIdNotCalled() {
            _mock.Verify(m => m.GetBy(It.IsAny<int>()), Times.Never);
        }

        public void VerifyGetByDescriptionNotCalled() {
            _mock.Verify(m => m.GetBy(It.IsAny<string>()), Times.Never);
        }

        public void VerifyDeleteCalled(int id, int times = 1) {
            _mock.Verify(m => m.Delete(id), Times.Exactly(times));
        }

        public void VerifyAddCalled(Category category, int times = 1) {
            _mock.Verify(m => m.Add(category), Times.Exactly(times));
        }

        public void VerifyAddNotCalled() {
            _mock.Verify(m => m.Add(It.IsAny<Category>()), Times.Never);
        }

        public void VerifyUpdateCalled(Category category, int times = 1) {
            _mock.Verify(m => m.Update(category), Times.Exactly(times));
        }

        public void VerifyUpdateNotCalled() {
            _mock.Verify(m => m.Update(It.IsAny<Category>()), Times.Never);
        }
    }
}
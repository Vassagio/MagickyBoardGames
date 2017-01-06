using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockRepository<T> : IRepository<T> where T: IEntity {
        private readonly Mock<IRepository<T>> _mock;

        public MockRepository() {
            _mock = new Mock<IRepository<T>>();
        }

        public Task<IEnumerable<T>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<T> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<T> GetBy(T entity) {
            return _mock.Object.GetBy(entity);
        }

        public Task<int> Add(T entity) {
            return _mock.Object.Add(entity);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public Task Update(T viewModel) {
            return _mock.Object.Update(viewModel);
        }
        public MockRepository<T> GetAllStubbedToReturn(IEnumerable<T> entities) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(entities));
            return this;
        }
        public MockRepository<T> GetByStubbedToReturn(T entity) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(entity));
            _mock.Setup(m => m.GetBy(It.IsAny<T>())).Returns(Task.FromResult(entity));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public void VerifyGetByCalled(int id, int times = 1) {
            _mock.Verify(m => m.GetBy(id), Times.Exactly(times));
        }

        public void VerifyGetByCalled(T entity, int times = 1) {
            _mock.Verify(m => m.GetBy(entity), Times.Exactly(times));
        }

        public void VerifyGetByNotCalled() {
            _mock.Verify(m => m.GetBy(It.IsAny<int>()), Times.Never);
        }

        public void VerifyDeleteCalled(int id, int times = 1) {
            _mock.Verify(m => m.Delete(id), Times.Exactly(times));
        }

        public void VerifyAddCalled(T viewModel, int times = 1) {
            _mock.Verify(m => m.Add(viewModel), Times.Exactly(times));
        }

        public void VerifyAddNotCalled() {
            _mock.Verify(m => m.Add(It.IsAny<T>()), Times.Never);
        }

        public void VerifyUpdateCalled(T viewModel, int times = 1) {
            _mock.Verify(m => m.Update(viewModel), Times.Exactly(times));
        }

        public void VerifyUpdateNotCalled() {
            _mock.Verify(m => m.Update(It.IsAny<T>()), Times.Never);
        }
    }
}
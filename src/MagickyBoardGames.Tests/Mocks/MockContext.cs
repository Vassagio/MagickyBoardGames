using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockContext<T> : IContext<T> where T: IViewModel {
        private readonly Mock<IContext<T>> _mock;

        public MockContext() {
            _mock = new Mock<IContext<T>>();
        }

        public Task<IEnumerable<T>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<T> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<int> Add(T viewModel) {
            return _mock.Object.Add(viewModel);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public Task Update(T viewModel) {
            return _mock.Object.Update(viewModel);
        }
        public MockContext<T> GetAllStubbedToReturn(IEnumerable<T> viewModels) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(viewModels));
            return this;
        }
        public MockContext<T> GetByStubbedToReturn(T viewModel) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(viewModel));
            return this;
        }

        public void VerifyGetAllCalled(int times = 1) {
            _mock.Verify(m => m.GetAll(), Times.Exactly(times));
        }

        public void VerifyGetByCalled(int id, int times = 1) {
            _mock.Verify(m => m.GetBy(id), Times.Exactly(times));
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
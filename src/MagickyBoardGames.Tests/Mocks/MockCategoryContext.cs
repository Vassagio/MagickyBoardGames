using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockCategoryContext : ICategoryContext {
        private readonly Mock<ICategoryContext> _mock;

        public MockCategoryContext() {
            _mock = new Mock<ICategoryContext>();
        }

        public Task<IEnumerable<CategoryViewModel>> GetAll() {
            return _mock.Object.GetAll();
        }

        public Task<CategoryViewModel> GetBy(int id) {
            return _mock.Object.GetBy(id);
        }

        public Task<int> Add(CategoryViewModel categoryViewModel) {
            return _mock.Object.Add(categoryViewModel);
        }

        public Task Delete(int id) {
            return _mock.Object.Delete(id);
        }

        public Task Update(CategoryViewModel categoryViewModel) {
            return _mock.Object.Update(categoryViewModel);
        }
        public MockCategoryContext GetAllStubbedToReturn(IEnumerable<CategoryViewModel> categoryViewModels) {
            _mock.Setup(m => m.GetAll()).Returns(Task.FromResult(categoryViewModels));
            return this;
        }
        public MockCategoryContext GetByStubbedToReturn(CategoryViewModel categoryViewModel) {
            _mock.Setup(m => m.GetBy(It.IsAny<int>())).Returns(Task.FromResult(categoryViewModel));
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

        public void VerifyAddCalled(CategoryViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Add(viewModel), Times.Exactly(times));
        }

        public void VerifyAddNotCalled() {
            _mock.Verify(m => m.Add(It.IsAny<CategoryViewModel>()), Times.Never);
        }

        public void VerifyUpdateCalled(CategoryViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Update(viewModel), Times.Exactly(times));
        }

        public void VerifyUpdateNotCalled() {
            _mock.Verify(m => m.Update(It.IsAny<CategoryViewModel>()), Times.Never);
        }
    }
}
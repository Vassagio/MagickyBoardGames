using System.Collections.Generic;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks
{
    public class MockBuilder<TEntity, TViewModel> : IBuilder<TEntity, TViewModel> where TEntity: IEntity where TViewModel:IViewModel{
        private readonly Mock<IBuilder<TEntity, TViewModel>> _mock;

        public MockBuilder() {
            _mock = new Mock<IBuilder<TEntity, TViewModel>>();
        }
        public TViewModel ToViewModel() {
            return _mock.Object.ToViewModel();
        }

        public TEntity ToEntity() {
            return _mock.Object.ToEntity();
        }

        public TViewModel Build(TEntity entity) {
            return _mock.Object.Build(entity);
        }

        public TEntity Build(TViewModel viewModel) {
            return _mock.Object.Build(viewModel);
        }

        public MockBuilder<TEntity, TViewModel> BuildStubbedToReturn(TViewModel viewModel) {
            _mock.Setup(m => m.Build(It.IsAny<TEntity>())).Returns(viewModel);
            return this;
        }

        public MockBuilder<TEntity, TViewModel> BuildStubbedToReturn(TEntity entity) {
            _mock.Setup(m => m.Build(It.IsAny<TViewModel>())).Returns(entity);
            return this;
        }

        public void VerifyBuildCalled(TEntity entity, int times = 1) {
            _mock.Verify(m => m.Build(entity), Times.Exactly(times));
        }

        public void VerifyBuildCalled(TViewModel viewModel, int times = 1) {
            _mock.Verify(m => m.Build(viewModel), Times.Exactly(times));
        }

        public void VerifyBuildNotCalled() {
            _mock.Verify(m => m.Build(It.IsAny<TEntity>()), Times.Never);
            _mock.Verify(m => m.Build(It.IsAny<TViewModel>()), Times.Never);
        }
    }
}

using System.Collections.Generic;
using System.Xml.Linq;
using MagickyBoardGames.Builders;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockXmlElementBuilder<TViewModel> : IXmlElementBuilder<TViewModel> where TViewModel : IViewModel {
        private readonly Mock<IXmlElementBuilder<TViewModel>> _mock;

        public MockXmlElementBuilder() {
            _mock = new Mock<IXmlElementBuilder<TViewModel>>();
        }

        public TViewModel ToViewModel() {
            return _mock.Object.ToViewModel();
        }

        public TViewModel Build(XElement element) {
            return _mock.Object.Build(element);
        }

        public MockXmlElementBuilder<TViewModel> BuildStubbedToReturn(params TViewModel[] viewModel) {
            var queue = new Queue<TViewModel>(viewModel);
            _mock.Setup(m => m.Build(It.IsAny<XElement>())).Returns(queue.Dequeue);
            return this;
        }

        public void VerifyBuildCalled(int times = 1) {
            _mock.Verify(m => m.Build(It.IsAny<XElement>()), Times.Exactly(times));
        }
    }
}
using System.Security.Claims;
using MagickyBoardGames.Services;
using Moq;

namespace MagickyBoardGames.Tests.Mocks
{
    public class MockApplicationUserManager : IApplicationUserManager
    {
        private readonly Mock<IApplicationUserManager> _mock;

        public MockApplicationUserManager() {
            _mock = new Mock<IApplicationUserManager>();
        }

        public string GetUserId(ClaimsPrincipal pricipal) {
            return _mock.Object.GetUserId(pricipal);
        }

        public MockApplicationUserManager GetUserIdStubbedToReturn(string id) {
            _mock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(id);
            return this;
        }

        public void VerifyGetUserIdCalled(ClaimsPrincipal pricipal, int times = 1) {
            _mock.Verify(m => m.GetUserId(pricipal), Times.Exactly(times));
        }

        public void VerifyGetUserIdNotCalled() {
            _mock.Verify(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }
    }
}

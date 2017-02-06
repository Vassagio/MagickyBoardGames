using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Services;
using MagickyBoardGames.Services.BoardGameGeek;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks
{
    public class MockGameInfoService : IGameInfoService
    {
        private readonly Mock<IGameInfoService> _mock;

        public MockGameInfoService() {
            _mock = new Mock<IGameInfoService>();
        }
        public Task<IEnumerable<SearchResult>> Search(string query) {
            return _mock.Object.Search(query);
        }

        public Task<IEnumerable<GameViewModel>> LoadGames(params int[] gameIds) {
            return _mock.Object.LoadGames(gameIds);
        }

        public Task<GameViewModel> LoadGame(int gameId) {
            return _mock.Object.LoadGame(gameId);
        }

        public MockGameInfoService SearchStubbedToReturn(IEnumerable<SearchResult> searchResults) {
            _mock.Setup(m => m.Search(It.IsAny<string>())).Returns(Task.FromResult(searchResults));
            return this;
        }

        public MockGameInfoService LoadGamesStubbedToReturn(IEnumerable<GameViewModel> games) {
            _mock.Setup(m => m.LoadGames(It.IsAny<int[]>())).Returns(Task.FromResult(games));
            return this;
        }

        public void VerifySearchCalled(string query, int times = 1) {
            _mock.Verify(m => m.Search(query), Times.Exactly(times));
        }

        public void VerifySearchNotCalled() {
            _mock.Verify(m => m.Search(It.IsAny<string>()), Times.Never);
        }

        public void VerifyLoadGamesCalled(int[] gameIds, int times = 1) {
            _mock.Verify(m => m.LoadGames(gameIds), Times.Exactly(times));
        }

        public void VerifyLoadGamesCalled() {
            _mock.Verify(m => m.LoadGames(It.IsAny<int[]>()), Times.Never);
        }
    }
}

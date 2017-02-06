using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Services;
using MagickyBoardGames.Services.BoardGameGeek;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameSearchContextTest {
        [Fact]
        public void Initialize_Game_Search_Context() {
            var context = BuildGameSearchContext();
            context.Should().NotBeNull();
        }
        
        [Fact]
        public async void Builds_A_View_Model() {
            var query = "Query";
            var viewModel = new ImportSearchViewModel {
                Query = query
            };
            var searchResults = new List<SearchResult> {
                new SearchResult {
                    GameId = 1
                }
            };
            var game = new GameViewModel();
            var games = new List<GameViewModel> {
                game
            };
            var gameInfoService = new MockGameInfoService().SearchStubbedToReturn(searchResults).LoadGamesStubbedToReturn(games);
            var context = BuildGameSearchContext(gameInfoService);

            var actual = await context.BuildViewModel(viewModel);

            actual.Should().NotBeNull();
            actual.Query.Should().Be(query);
            actual.BoardGames.Count().Should().Be(1);
            actual.BoardGames.First().Should().Be(game);
            gameInfoService.VerifySearchCalled(query);
            gameInfoService.VerifyLoadGamesCalled(new List<int> {1}.ToArray());
        }

        private static GameSearchContext BuildGameSearchContext(IGameInfoService gameInfoService = null) {
            gameInfoService = gameInfoService ?? new MockGameInfoService();
            return new GameSearchContext(gameInfoService);
        }
    }
}
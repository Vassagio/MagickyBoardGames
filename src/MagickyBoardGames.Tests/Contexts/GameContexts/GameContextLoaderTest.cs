using FluentAssertions;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameContextLoaderTest {
        [Fact]
        public void Initializes() {
            var contextLoader = BuildContextLoader();
            contextLoader.Should().NotBeNull();
            contextLoader.Should().BeAssignableTo<IGameContextLoader>();
        }

        [Fact]
        public void Loads_Game_List_Context() {
            var gameListContext = new MockGameListContext();
            var contextLoader = BuildContextLoader(gameListContext);

            var context = contextLoader.LoadGameListContext();

            context.Should().Be(gameListContext);
        }

        [Fact]
        public void Loads_Game_View_Context() {
            var gameViewContext = new MockGameViewContext();
            var contextLoader = BuildContextLoader(gameViewContext: gameViewContext);

            var context = contextLoader.LoadGameViewContext();

            context.Should().Be(gameViewContext);
        }

        [Fact]
        public void Loads_Game_Save_Context() {
            var gameSaveContext = new MockGameSaveContext();
            var contextLoader = BuildContextLoader(gameSaveContext: gameSaveContext);

            var context = contextLoader.LoadGameSaveContext();

            context.Should().Be(gameSaveContext);
        }

        private static GameContextLoader BuildContextLoader(IGameListContext gameListContext = null, IGameViewContext gameViewContext = null, IGameSaveContext gameSaveContext = null) {
            gameListContext = gameListContext ?? new MockGameListContext();
            gameViewContext = gameViewContext ?? new MockGameViewContext();
            gameSaveContext = gameSaveContext ?? new MockGameSaveContext();
            return new GameContextLoader(gameListContext, gameViewContext, gameSaveContext);
        }
    }
}
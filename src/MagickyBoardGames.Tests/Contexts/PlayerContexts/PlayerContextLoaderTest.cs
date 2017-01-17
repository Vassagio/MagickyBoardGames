using FluentAssertions;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts;
using MagickyBoardGames.Tests.Mocks.MockContexts.Player;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.PlayerContexts {
    public class PlayerContextLoaderTest {
        [Fact]
        public void Initializes() {
            var contextLoader = BuildContextLoader();
            contextLoader.Should().NotBeNull();
            contextLoader.Should().BeAssignableTo<IPlayerContextLoader>();
        }

        [Fact]
        public void Loads_Player_List_Context() {
            var playerListContext = new MockPlayerListContext();
            var contextLoader = BuildContextLoader(playerListContext);

            var context = contextLoader.LoadPlayerListContext();

            context.Should().Be(playerListContext);
        }

        [Fact]
        public void Loads_Player_View_Context() {
            var playerViewContext = new MockPlayerViewContext();
            var contextLoader = BuildContextLoader(playerViewContext: playerViewContext);

            var context = contextLoader.LoadPlayerViewContext();

            context.Should().Be(playerViewContext);
        }

        private static PlayerContextLoader BuildContextLoader(IPlayerListContext playerListContext = null, IPlayerViewContext playerViewContext = null) {
            playerListContext = playerListContext ?? new MockPlayerListContext();
            playerViewContext = playerViewContext ?? new MockPlayerViewContext();
            return new PlayerContextLoader(playerListContext, playerViewContext);
        }
    }
}
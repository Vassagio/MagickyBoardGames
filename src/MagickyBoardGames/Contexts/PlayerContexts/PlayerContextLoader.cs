namespace MagickyBoardGames.Contexts.PlayerContexts
{
    public class PlayerContextLoader : IPlayerContextLoader
    {
        private readonly IPlayerListContext _playerListContext;
        private readonly IPlayerViewContext _playerViewContext;

        public PlayerContextLoader(IPlayerListContext playerListContext, IPlayerViewContext playerViewContext) {
            _playerListContext = playerListContext;
            _playerViewContext = playerViewContext;
        }

        public IPlayerListContext LoadPlayerListContext() {
            return _playerListContext;
        }

        public IPlayerViewContext LoadPlayerViewContext() {
            return _playerViewContext;
        }
    }
}

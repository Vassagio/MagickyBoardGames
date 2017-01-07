namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameContextLoader : IGameContextLoader {
        private readonly IGameListContext _gameListContext;
        private readonly IGameViewContext _gameViewContext;
        private readonly IGameSaveContext _gameSaveContext;

        public GameContextLoader(IGameListContext gameListContext, IGameViewContext gameViewContext, IGameSaveContext gameSaveContext) {
            _gameListContext = gameListContext;
            _gameViewContext = gameViewContext;
            _gameSaveContext = gameSaveContext;
        }

        public IGameListContext LoadGameListContext() {
            return _gameListContext;
        }

        public IGameViewContext LoadGameViewContext() {
            return _gameViewContext;
        }

        public IGameSaveContext LoadGameSaveContext() {
            return _gameSaveContext;
        }
    }
}

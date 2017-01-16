namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameContextLoader : IGameContextLoader {
        private readonly IGameListContext _gameListContext;
        private readonly IGameViewContext _gameViewContext;
        private readonly IGameSaveContext _gameSaveContext;
        private readonly IGameRateContext _gameRateContext;

        public GameContextLoader(IGameListContext gameListContext, IGameViewContext gameViewContext, IGameSaveContext gameSaveContext, IGameRateContext gameRateContext) {
            _gameListContext = gameListContext;
            _gameViewContext = gameViewContext;
            _gameSaveContext = gameSaveContext;
            _gameRateContext = gameRateContext;
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

        public IGameRateContext LoadGameRateContext() {
            return _gameRateContext;
        }
    }
}

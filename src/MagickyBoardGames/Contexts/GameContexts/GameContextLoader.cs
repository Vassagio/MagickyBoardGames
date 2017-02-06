namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameContextLoader : IGameContextLoader {
        private readonly IGameListContext _gameListContext;
        private readonly IGameViewContext _gameViewContext;
        private readonly IGameSaveContext _gameSaveContext;
        private readonly IGameRateContext _gameRateContext;
        private readonly IGameSearchContext _gameSearchContext;

        public GameContextLoader(IGameListContext gameListContext, IGameViewContext gameViewContext, IGameSaveContext gameSaveContext, IGameRateContext gameRateContext, IGameSearchContext gameSearchContext) {
            _gameListContext = gameListContext;
            _gameViewContext = gameViewContext;
            _gameSaveContext = gameSaveContext;
            _gameRateContext = gameRateContext;
            _gameSearchContext = gameSearchContext;
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

        public IGameSearchContext LoadGameSearchContext() {
            return _gameSearchContext;
        }
    }
}

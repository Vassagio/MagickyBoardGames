namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameContextLoader {
        IGameListContext LoadGameListContext();
        IGameViewContext LoadGameViewContext();
        IGameSaveContext LoadGameSaveContext();
        IGameRateContext LoadGameRateContext();
        IGameSearchContext LoadGameSearchContext();
    }
}
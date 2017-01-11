namespace MagickyBoardGames.Contexts.PlayerContexts
{
    public interface IPlayerContextLoader
    {
        IPlayerListContext LoadPlayerListContext();
        IPlayerViewContext LoadPlayerViewContext();
    }
}

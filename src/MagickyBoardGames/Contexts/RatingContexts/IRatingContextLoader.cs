namespace MagickyBoardGames.Contexts.RatingContexts
{
    public interface IRatingContextLoader
    {
        IRatingListContext LoadRatingListContext();
        IRatingViewContext LoadRatingViewContext();
    }
}

namespace MagickyBoardGames.Contexts.RatingContexts
{
    public class RatingContextLoader : IRatingContextLoader
    {
        private readonly IRatingListContext _ratingListContext;
        private readonly IRatingViewContext _ratingViewContext;

        public RatingContextLoader(IRatingListContext ratingListContext, IRatingViewContext ratingViewContext) {
            _ratingListContext = ratingListContext;
            _ratingViewContext = ratingViewContext;
        }

        public IRatingListContext LoadRatingListContext() {
            return _ratingListContext;
        }

        public IRatingViewContext LoadRatingViewContext() {
            return _ratingViewContext;
        }
    }
}

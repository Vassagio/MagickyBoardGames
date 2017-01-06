using MagickyBoardGames.Contexts.CategoryContexts;

namespace MagickyBoardGames.Contexts
{
    public class ContextLoader : IContextLoader
    {
        private readonly ICategoryIndexContext _categoryIndexContext;
        private readonly ICategoryDetailContext _categoryDetailContext;

        public ContextLoader(ICategoryIndexContext categoryIndexContext, ICategoryDetailContext categoryDetailContext) {
            _categoryIndexContext = categoryIndexContext;
            _categoryDetailContext = categoryDetailContext;
        }

        public ICategoryIndexContext LoadCategoryIndexContext() {
            return _categoryIndexContext;
        }

        public ICategoryDetailContext LoadCategoryDetailContext() {
            return _categoryDetailContext;
        }
    }
}

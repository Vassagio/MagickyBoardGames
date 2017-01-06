using MagickyBoardGames.Contexts.CategoryContexts;

namespace MagickyBoardGames.Contexts
{
    public class ContextLoader : IContextLoader
    {
        private readonly ICategoryListContext _categoryListContext;
        private readonly ICategoryViewContext _categoryViewContext;

        public ContextLoader(ICategoryListContext categoryListContext, ICategoryViewContext categoryViewContext) {
            _categoryListContext = categoryListContext;
            _categoryViewContext = categoryViewContext;
        }

        public ICategoryListContext LoadCategoryListContext() {
            return _categoryListContext;
        }

        public ICategoryViewContext LoadCategoryViewContext() {
            return _categoryViewContext;
        }
    }
}

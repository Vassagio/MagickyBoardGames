using MagickyBoardGames.Contexts.CategoryContexts;

namespace MagickyBoardGames.Contexts
{
    public class ContextLoader : IContextLoader
    {
        private readonly ICategoryListContext _categoryListContext;
        private readonly ICategoryViewContext _categoryViewContext;
        private readonly ICategorySaveContext _categorySaveContext;

        public ContextLoader(ICategoryListContext categoryListContext, ICategoryViewContext categoryViewContext, ICategorySaveContext categorySaveContext) {
            _categoryListContext = categoryListContext;
            _categoryViewContext = categoryViewContext;
            _categorySaveContext = categorySaveContext;
        }

        public ICategoryListContext LoadCategoryListContext() {
            return _categoryListContext;
        }

        public ICategoryViewContext LoadCategoryViewContext() {
            return _categoryViewContext;
        }

        public ICategorySaveContext LoadCategorySaveContext() {
            return _categorySaveContext;
        }
    }
}

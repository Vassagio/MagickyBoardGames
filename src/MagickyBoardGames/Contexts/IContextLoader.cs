using MagickyBoardGames.Contexts.CategoryContexts;

namespace MagickyBoardGames.Contexts
{
    public interface IContextLoader
    {
        ICategoryListContext LoadCategoryListContext();
        ICategoryViewContext LoadCategoryViewContext();
        ICategorySaveContext LoadCategorySaveContext();
    }
}

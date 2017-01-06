using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts
{
    public interface IContextLoader
    {
        ICategoryListContext LoadCategoryListContext();
        ICategoryViewContext LoadCategoryViewContext();
    }
}

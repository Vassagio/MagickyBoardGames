using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts
{
    public interface IContextLoader
    {
        ICategoryIndexContext LoadCategoryIndexContext();
        ICategoryDetailContext LoadCategoryDetailContext();
    }
}

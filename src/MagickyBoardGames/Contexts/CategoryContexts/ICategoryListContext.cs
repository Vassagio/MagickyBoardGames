using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public interface ICategoryListContext {
        Task<CategoryListViewModel> BuildViewModel();
    }
}
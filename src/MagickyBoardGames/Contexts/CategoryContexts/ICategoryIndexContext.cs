using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public interface ICategoryIndexContext {
        Task<CategoryIndexViewModel> BuildViewModel();
    }
}
using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public interface ICategoryViewContext {
        Task<CategoryViewViewModel> BuildViewModel(int id);
        Task Delete(int id);
    }
}
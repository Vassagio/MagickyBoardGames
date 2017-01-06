using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public interface ICategoryDetailContext {
        Task<CategoryDetailViewModel> BuildViewModel(int id);
    }
}
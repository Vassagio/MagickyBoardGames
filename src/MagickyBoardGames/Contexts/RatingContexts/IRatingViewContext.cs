using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.RatingContexts {
    public interface IRatingViewContext {
        Task<RatingViewViewModel> BuildViewModel(int id);
    }
}
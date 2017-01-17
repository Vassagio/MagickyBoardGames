using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.RatingContexts {
    public interface IRatingListContext {
        Task<RatingListViewModel> BuildViewModel();
    }
}
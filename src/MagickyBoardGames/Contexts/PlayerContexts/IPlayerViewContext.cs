using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.PlayerContexts {
    public interface IPlayerViewContext {
        Task<PlayerViewViewModel> BuildViewModel(string id);
    }
}
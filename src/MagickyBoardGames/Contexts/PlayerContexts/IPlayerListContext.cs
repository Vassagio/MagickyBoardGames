using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.PlayerContexts {
    public interface IPlayerListContext {
        Task<PlayerListViewModel> BuildViewModel();
    }
}
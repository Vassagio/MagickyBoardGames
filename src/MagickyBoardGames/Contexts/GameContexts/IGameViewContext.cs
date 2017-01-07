using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameViewContext {
        Task<GameViewViewModel> BuildViewModel(int id);
        Task Delete(int id);
    }
}
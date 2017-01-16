using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameRateContext {
        Task<GameRateViewModel> BuildViewModel(int gameId, string playerId);
        Task Save(GameRateViewModel viewModel);
    }
}
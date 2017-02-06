using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameSearchContext {
        Task<ImportSearchViewModel> BuildViewModel(ImportSearchViewModel viewModel);
    }
}
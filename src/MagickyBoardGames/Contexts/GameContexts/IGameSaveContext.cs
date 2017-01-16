using System.Threading.Tasks;
using FluentValidation.Results;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameSaveContext {
        ValidationResult Validate(GameSaveViewModel viewModel);
        Task Save(GameSaveViewModel viewModel);
        Task<GameSaveViewModel> BuildViewModel(int id, string userId);
        Task<GameSaveViewModel> BuildViewModel();
    }
}
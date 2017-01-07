using System.Threading.Tasks;
using FluentValidation.Results;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public interface IGameSaveContext {
        ValidationResult Validate(GameViewModel viewModel);
        Task Save(GameViewModel viewModel);
    }
}
using System.Threading.Tasks;
using FluentValidation.Results;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.CategoryContexts {
    public interface ICategorySaveContext {
        ValidationResult Validate(CategoryViewModel viewModel);
        Task Save(CategoryViewModel viewModel);
    }
}
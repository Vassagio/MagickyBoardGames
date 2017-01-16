using FluentValidation;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Validations {
    public class GameSaveViewModelValidator : AbstractValidator<GameSaveViewModel> {
        public GameSaveViewModelValidator() {
            RuleFor(x => x.Game.Name)
                .NotEmpty().WithMessage("Must have a name.")
                .Length(1, 100).WithMessage("Must be less than or equal to 100 characters.");
            RuleFor(x => x.Game.MinPlayers)
                .GreaterThan(0).WithMessage("Min Players must be greater than 0.");
            RuleFor(x => x.Game.MaxPlayers)
                .GreaterThan(0).WithMessage("Max Players must be greater than 0.")
                .GreaterThanOrEqualTo(x => x.Game.MinPlayers).WithMessage("Must be greater than or equal to min players.");
        }
    }
}
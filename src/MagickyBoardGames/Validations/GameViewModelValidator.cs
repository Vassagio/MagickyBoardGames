using FluentValidation;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Validations {
    public class GameViewModelValidator : AbstractValidator<GameViewModel> {
        public GameViewModelValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Must have a name.")
                .Length(1, 100).WithMessage("Must be less than or equal to 100 characters.");
            RuleFor(x => x.MinPlayers)
                .GreaterThan(0).WithMessage("Min Players must be greater than 0.");
            RuleFor(x => x.MaxPlayers)
                .GreaterThan(0).WithMessage("Max Players must be greater than 0.")
                .GreaterThan(x => x.MinPlayers).WithMessage("Must be greater than min players.");
        }
    }
}
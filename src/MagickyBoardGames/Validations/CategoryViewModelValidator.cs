using FluentValidation;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Validations {
    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel> {
        public CategoryViewModelValidator() {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Must have a description.");
            RuleFor(x => x.Description).Length(1, 30).WithMessage("Must be less than or equal to 30 characters.");
        }
    }
}
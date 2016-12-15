using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.Models.AccountViewModels {
    public class ExternalLoginConfirmationViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
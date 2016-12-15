﻿using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.Models.AccountViewModels {
    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
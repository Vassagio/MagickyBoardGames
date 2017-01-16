using System.Security.Claims;
using MagickyBoardGames.Models;
using Microsoft.AspNetCore.Identity;

namespace MagickyBoardGames.Services {
    public class ApplicationUserManager : IApplicationUserManager {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal pricipal) {
            return _userManager.GetUserId(pricipal);
        }
    }
}
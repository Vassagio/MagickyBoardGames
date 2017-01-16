using System.Security.Claims;

namespace MagickyBoardGames.Services {
    public interface IApplicationUserManager {
        string GetUserId(ClaimsPrincipal pricipal);
    }
}
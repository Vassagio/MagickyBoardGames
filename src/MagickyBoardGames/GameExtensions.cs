using System.Collections.Generic;
using System.Linq;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

//TODO: Needs Testing
namespace MagickyBoardGames {
    public static class GameExtensions {
        public static IGameSearchQuery ToSearchableQuery(this IEnumerable<Game> games) {
            return new GameSearchQuery(games.AsQueryable());
        }

        public static IQueryable<Game> IncludeAllAssociations(this IQueryable<Game> games) {
            return games.Include(g => g.GameCategories).ThenInclude(gc => gc.Category)
                .Include(g => g.GameOwners).ThenInclude(go => go.Owner)
                .Include(g => g.GamePlayerRatings).ThenInclude(gpr => gpr.Rating);
        }
    }
}
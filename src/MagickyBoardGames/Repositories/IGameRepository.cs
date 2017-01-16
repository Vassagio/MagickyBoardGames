using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<Game> GetBy(string name);
        Task<int> Add(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string playerId);
        Task Update(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners, int ratingId, string playerId);
    }
}

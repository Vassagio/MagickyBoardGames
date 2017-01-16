using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface IRatingRepository {
        Task<IEnumerable<Rating>> GetAll();
        Task<Rating> GetBy(int id);
        Task<Rating> GetBy(string name);
    }
}
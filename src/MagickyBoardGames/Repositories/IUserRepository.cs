using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface IUserRepository {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> GetBy(string name);
    }
}
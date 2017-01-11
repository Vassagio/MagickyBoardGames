using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface IGameOwnerRepository {
        Task Adjust(int id, IEnumerable<ApplicationUser> owners);
    }
}
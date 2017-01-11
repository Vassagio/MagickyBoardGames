using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface IGameCategoryRepository {
        Task Adjust(int id, IEnumerable<Category> categories);
    }
}
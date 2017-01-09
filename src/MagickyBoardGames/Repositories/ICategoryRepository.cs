using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface ICategoryRepository: IRepository<Category> {
        Task<Category> GetBy(string description);
    }
}
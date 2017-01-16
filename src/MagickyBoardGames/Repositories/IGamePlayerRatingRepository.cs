using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories {
    public interface IGamePlayerRatingRepository {
        Task Save(int gameId, string playerId, int ratingId);
        Task<GamePlayerRating> GetBy(int gameId, string playerId);
    }
}
using System.Threading.Tasks;

namespace MagickyBoardGames.Repositories {
    public interface IGamePlayerRatingRepository {
        Task Save(int gameId, string playerId, int ratingId);
    }
}
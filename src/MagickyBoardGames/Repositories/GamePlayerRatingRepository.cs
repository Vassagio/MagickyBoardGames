using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories {
    public class GamePlayerRatingRepository : IGamePlayerRatingRepository {
        private readonly ApplicationDbContext _context;

        public GamePlayerRatingRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task Save(int gameId, string playerId, int ratingId) {
            var gamePlayerRating = await _context.GamePlayerRatings.SingleOrDefaultAsync(gpr => gpr.GameId == gameId && gpr.PlayerId == playerId);
            if (gamePlayerRating == null)
                await Add(gameId, playerId, ratingId);
            else
                Update(gamePlayerRating, ratingId);
            await _context.SaveChangesAsync();
        }

        private async Task Add(int gameId, string playerId, int ratingId) {
            var gamePlayerRating = new GamePlayerRating { GameId = gameId, PlayerId = playerId, RatingId = ratingId };
            await _context.GamePlayerRatings.AddAsync(gamePlayerRating);
        }

        private void Update(GamePlayerRating gamePlayerRating, int ratingId) {
            gamePlayerRating.RatingId = ratingId;
            _context.GamePlayerRatings.Update(gamePlayerRating);
        }
    }
}
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

        public async Task Save(int gameId, string playerId, int rating) {
            var gamePlayerRating = await _context.GamePlayerRatings.SingleOrDefaultAsync(gpr => gpr.GameId == gameId);
            if (gamePlayerRating == null)
                await Add(gameId, playerId, rating);
            else
                Update(gamePlayerRating, rating);
            await _context.SaveChangesAsync();
        }

        private async Task Add(int gameId, string playerId, int rating) {
            var gamePlayerRating = new GamePlayerRating { GameId = gameId, PlayerId = playerId, Rating = rating };
            await _context.GamePlayerRatings.AddAsync(gamePlayerRating);            
        }

        private void Update(GamePlayerRating gamePlayerRating, int rating) {
            gamePlayerRating.Rating = rating;
            _context.GamePlayerRatings.Update(gamePlayerRating);
        }
    }
}
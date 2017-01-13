using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories
{
    public class RatingRepository : IRatingRepository {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAll() {
            return await _context.Ratings.ToListAsync();
        }

        private IQueryable<Rating> Ratings() {
            return _context.Ratings.Include(c => c.GamePlayerRatings).ThenInclude(gpr => gpr.Game);
        }

        public async Task<Rating> GetBy(int id) {
            return await Ratings().SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Rating> GetBy(string description) {
            return await Ratings().SingleOrDefaultAsync(c => c.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

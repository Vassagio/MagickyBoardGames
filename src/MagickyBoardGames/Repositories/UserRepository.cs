using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories
{
    public class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll() {
            return await _context.Users.ToListAsync();
        }

        private IQueryable<ApplicationUser> Users() {
            return _context.Users.Include(u => u.GameOwners).ThenInclude(go => go.Game)
                                 .Include(u => u.GamePlayerRatings);
        }

        public async Task<ApplicationUser> GetById(string id) {
            return await Users().SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ApplicationUser> GetBy(string name) {
            return await Users().SingleOrDefaultAsync(c => c.UserName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

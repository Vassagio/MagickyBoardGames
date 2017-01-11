using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories {
    public class GameOwnerRepository : IGameOwnerRepository {
        private readonly ApplicationDbContext _context;

        public GameOwnerRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task Adjust(int id, IEnumerable<ApplicationUser> owners) {
            Remove(id, owners);
            await Add(id, owners);
            await _context.SaveChangesAsync();
        }

        private async Task Add(int id, IEnumerable<ApplicationUser> owners) {
            foreach (var owner in owners) {
                var exists = await _context.GameOwners.AnyAsync(gc => gc.GameId == id && gc.OwnerId == owner.Id);
                if (exists)
                    continue;

                var gameOwner = new GameOwner {
                    GameId = id,
                    OwnerId = owner.Id
                };
                await _context.GameOwners.AddAsync(gameOwner);
            }
        }

        private void Remove(int id, IEnumerable<ApplicationUser> owners) {
            foreach (var gameOwner in _context.GameOwners
                .Where(gc => gc.GameId == id && !(owners.Any(c => c.Id == gc.OwnerId))))
                _context.GameOwners.Remove(gameOwner);
        }
    }
}

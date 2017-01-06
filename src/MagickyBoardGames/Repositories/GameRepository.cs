using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories {
    public class GameRepository : IRepository<Game> {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAll() {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game> GetBy(int id) {
            return await _context.Games.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Game> GetBy(Game game) {
            return await _context.Games.SingleOrDefaultAsync(c => c.Name.Equals(game.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<int> Add(Game entity) {
            if (string.IsNullOrEmpty(entity.Name))
                throw new ArgumentException();

            await _context.Games.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var category = await GetBy(id);
            if (category != null)
                _context.Games.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Game entity) {
            if (string.IsNullOrEmpty(entity.Name))
                throw new ArgumentException();

            if (!await Exists(entity.Id))
                throw new ArgumentException();

            _context.Games.Update(entity);
            await _context.SaveChangesAsync();
        }

        public Task<Category> GetBy(string description) {
            throw new NotImplementedException();
        }

        private async Task<bool> Exists(int id) {
            return await _context.Games.AnyAsync(c => c.Id == id);
        }
    }
}
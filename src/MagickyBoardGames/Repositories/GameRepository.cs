using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories {
    public class GameRepository : IGameRepository {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAll() {
            return await _context.Games.ToListAsync();
        }

        private IQueryable<Game> Games() {
            return _context.Games.Include(g => g.GameCategories).ThenInclude(gc => gc.Category);
        }

        public async Task<Game> GetBy(int id) {       
            return await Games().SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Game> GetBy(string name) {
            return await Games().SingleOrDefaultAsync(c => c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<int> Add(Game game) {
            if (string.IsNullOrEmpty(game.Name))
                throw new ArgumentException();

            await _context.Games.AddAsync(game);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Add(Game game, IEnumerable<Category> categories) {
            var id = await Add(game);

            await AdjustGameCategories(id, categories);
            return id;
        }

        private async Task AdjustGameCategories(int id, IEnumerable<Category> categories) {
            RemoveGameCategories(id, categories);
            await AddGameCategories(id, categories);
            await _context.SaveChangesAsync();
        }

        private async Task AddGameCategories(int id, IEnumerable<Category> categories) {
            foreach (var category in categories) {
                var exists = await _context.GameCategories.AnyAsync(gc => gc.GameId == id && gc.CategoryId == category.Id);
                if (exists)
                    continue;

                var gameCategory = new GameCategory {
                    GameId = id,
                    CategoryId = category.Id
                };
                await _context.GameCategories.AddAsync(gameCategory);
            }
        }

        private void RemoveGameCategories(int id, IEnumerable<Category> categories) {
            foreach (var gameCategory in _context.GameCategories
                .Where(gc => gc.GameId == id && !(categories.Any(c => c.Id == gc.CategoryId))))
                _context.GameCategories.Remove(gameCategory);
        }

        public async Task Delete(int id) {
            var category = await GetBy(id);
            if (category != null)
                _context.Games.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Game game) {
            if (string.IsNullOrEmpty(game.Name))
                throw new ArgumentException();

            if (!await Exists(game.Id))
                throw new ArgumentException();

            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Game game, IEnumerable<Category> categories) {
            await Update(game);
            await AdjustGameCategories(game.Id, categories);
        }

        private async Task<bool> Exists(int id) {
            return await _context.Games.AnyAsync(c => c.Id == id);
        }
    }
}
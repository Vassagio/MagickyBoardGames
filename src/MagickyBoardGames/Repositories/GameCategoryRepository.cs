using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories
{
    public class GameCategoryRepository : IGameCategoryRepository {
        private readonly ApplicationDbContext _context;

        public GameCategoryRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task Adjust(int id, IEnumerable<Category> categories) {
            Remove(id, categories);
            await Add(id, categories);
            await _context.SaveChangesAsync();
        }

        private async Task Add(int id, IEnumerable<Category> categories) {
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

        private void Remove(int id, IEnumerable<Category> categories) {
            foreach (var gameCategory in _context.GameCategories
                .Where(gc => gc.GameId == id && !(categories.Any(c => c.Id == gc.CategoryId))))
                _context.GameCategories.Remove(gameCategory);
        }
    }
}

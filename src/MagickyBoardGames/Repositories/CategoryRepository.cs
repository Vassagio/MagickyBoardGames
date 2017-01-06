using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Repositories
{
    public class CategoryRepository: IRepository<Category>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll() {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetBy(int id) {
            return await _context.Categories.Include(c => c.GameCategories).ThenInclude(gc => gc.Game).SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> GetBy(Category category) {
            return await _context.Categories.Include(c => c.GameCategories).ThenInclude(gc => gc.Game).SingleOrDefaultAsync(c => c.Description.Equals(category.Description, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<int> Add(Category entity) {
            if (string.IsNullOrEmpty(entity.Description))
                throw new ArgumentException();

            await _context.Categories.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var category = await GetBy(id);
            if (category != null)
                _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category entity) {
            if (string.IsNullOrEmpty(entity.Description))
                throw new ArgumentException();

            if (!await Exists(entity.Id))
                throw new ArgumentException();

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> Exists(int id) {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}

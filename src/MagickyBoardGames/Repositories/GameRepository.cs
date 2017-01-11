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
        private readonly IGameCategoryRepository _gameCategoryRepository;
        private readonly IGameOwnerRepository _gameOwnerRespository;

        public GameRepository(ApplicationDbContext context, IGameCategoryRepository gameCategoryRepository, IGameOwnerRepository gameOwnerRespository) {
            _context = context;
            _gameCategoryRepository = gameCategoryRepository;
            _gameOwnerRespository = gameOwnerRespository;
        }

        public async Task<IEnumerable<Game>> GetAll() {
            return await _context.Games.ToListAsync();
        }

        private IQueryable<Game> Games() {
            return _context.Games
                .Include(g => g.GameCategories).ThenInclude(gc => gc.Category)
                .Include(g => g.GameOwners).ThenInclude(go => go.Owner);
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

        public async Task<int> Add(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners) {
            var id = await Add(game);

            await _gameCategoryRepository.Adjust(game.Id, categories);
            await _gameOwnerRespository.Adjust(game.Id, owners);
            return id;
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

        public async Task Update(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners) {
            await Update(game);
            await _gameCategoryRepository.Adjust(game.Id, categories);
            await _gameOwnerRespository.Adjust(game.Id, owners);
        }

        private async Task<bool> Exists(int id) {
            return await _context.Games.AnyAsync(c => c.Id == id);
        }
    }
}
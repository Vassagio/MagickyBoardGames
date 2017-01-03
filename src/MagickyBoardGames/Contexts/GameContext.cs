using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Contexts {
    public class GameContext : IContext<GameViewModel> {
        private readonly ApplicationDbContext _context;
        private readonly GameBuilder _gameBuilder;

        public GameContext(ApplicationDbContext context, GameBuilder gameBuilder) {
            _context = context;
            _gameBuilder = gameBuilder;
        }
        public async Task<IEnumerable<GameViewModel>> GetAll() {
            return await _context.Games.Select(game => _gameBuilder.Build(game))
                .ToListAsync();
        }

        public async Task<GameViewModel> GetBy(int id) {
            return await _context.Games.Select(game => _gameBuilder.Build(game))
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Add(GameViewModel viewModel) {
            if (string.IsNullOrEmpty(viewModel.Name) || !viewModel.MinPlayers.HasValue || !viewModel.MaxPlayers.HasValue)
                throw new ArgumentException();

            var game = new Game {
                Name = viewModel.Name,
                Description = viewModel.Description,
                MinPlayers = viewModel.MinPlayers.Value,
                MaxPlayers = viewModel.MaxPlayers.Value
            };
            await _context.Games.AddAsync(game);
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var game = await _context.Games.SingleOrDefaultAsync(c => c.Id == id);
            if (game != null)
                _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task Update(GameViewModel viewModel) {
            if (string.IsNullOrEmpty(viewModel.Name) || !viewModel.MinPlayers.HasValue || !viewModel.MaxPlayers.HasValue)
                throw new ArgumentException();

            var game = await _context.Games.SingleOrDefaultAsync(c => c.Id == viewModel.Id);
            if (game == null)
                throw new ArgumentException();

            game.Name = viewModel.Name;
            game.Description = viewModel.Description;
            game.MinPlayers = viewModel.MinPlayers.Value;
            game.MaxPlayers = viewModel.MaxPlayers.Value;
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }
    }
}

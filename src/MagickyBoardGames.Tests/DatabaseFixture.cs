using System;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Tests {
    public class DatabaseFixture : IDisposable {
        public ApplicationDbContext Db { get; }

        public DatabaseFixture() {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseInMemoryDatabase();
            Db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public async Task Populate() {
            foreach (var category in Db.Categories)
                Db.Categories.Remove(category);
            await Db.SaveChangesAsync();
            foreach (var game in Db.Games)
                Db.Games.Remove(game);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params Category[] categories) {
            foreach (var category in Db.Categories)
                Db.Categories.Remove(category);
            await Db.SaveChangesAsync();
            foreach (var category in categories)
                await Db.Categories.AddAsync(category);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params Game[] games) {
            foreach (var game in Db.Games)
                Db.Games.Remove(game);
            await Db.SaveChangesAsync();
            foreach (var game in games)
                await Db.Games.AddAsync(game);
            await Db.SaveChangesAsync();                        
        }

        public void Dispose() {
            Db.Dispose();
        }

 
    }
}
using System;
using MagickyBoardGames.Data;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Tests {
    public class DatabaseFixture : IDisposable {
        public ApplicationDbContext Db { get; }

        public DatabaseFixture() {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseInMemoryDatabase();
            Db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public void Dispose() {
            Db.Dispose();
        }
    }
}
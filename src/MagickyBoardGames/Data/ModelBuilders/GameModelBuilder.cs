using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders {
    public class GameModelBuilder {
        public static void Build(ModelBuilder builder) {
            builder.Entity<Game>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            builder.Entity<Game>().Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<Game>().HasIndex(c => c.Name).ForSqlServerIsClustered().IsUnique();
            builder.Entity<Game>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Game>().Property(c => c.Description).IsRequired(false);
            builder.Entity<Game>().Property(c => c.MinPlayers).IsRequired();
            builder.Entity<Game>().Property(c => c.MaxPlayers).IsRequired();
        }
    }
}
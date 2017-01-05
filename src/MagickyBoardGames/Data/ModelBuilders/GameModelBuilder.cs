using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders {
    public class GameModelBuilder : BaseModelBuilder {
        public GameModelBuilder(ModelBuilder builder) : base(builder) {}

        public override void Build() {
            Builder.Entity<Game>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            Builder.Entity<Game>().Property(c => c.Id).UseSqlServerIdentityColumn();
            Builder.Entity<Game>().HasIndex(c => c.Name).ForSqlServerIsClustered().IsUnique();
            Builder.Entity<Game>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            Builder.Entity<Game>().Property(c => c.Description).IsRequired(false);
            Builder.Entity<Game>().Property(c => c.MinPlayers).IsRequired();
            Builder.Entity<Game>().Property(c => c.MaxPlayers).IsRequired();
        }
    }
}
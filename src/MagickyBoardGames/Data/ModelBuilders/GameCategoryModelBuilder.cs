using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders {
    public class GameCategoryModelBuilder : BaseModelBuilder {
        public GameCategoryModelBuilder(ModelBuilder builder) : base(builder) { }

        public override void Build() {
            Builder.Entity<GameCategory>().HasKey(gc => gc.Id).ForSqlServerIsClustered(false);
            Builder.Entity<GameCategory>().Property(c => c.Id).UseSqlServerIdentityColumn();
            Builder.Entity<GameCategory>().HasIndex(c => new {
                c.GameId,
                c.CategoryId
            }).IsUnique().ForSqlServerIsClustered();
            Builder.Entity<GameCategory>().Property(gc => gc.GameId).IsRequired();
            Builder.Entity<GameCategory>().Property(gc => gc.CategoryId).IsRequired();
            Builder.Entity<GameCategory>().HasOne(gc => gc.Game).WithMany(g => g.GameCategories).HasForeignKey(gc => gc.GameId);
            Builder.Entity<GameCategory>().HasOne(gc => gc.Category).WithMany(c => c.GameCategories).HasForeignKey(gc => gc.CategoryId);
        }
    }
}
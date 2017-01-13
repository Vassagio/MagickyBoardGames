using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders {
    public class GamePlayerRatingModelBuilder : BaseModelBuilder {
        public GamePlayerRatingModelBuilder(ModelBuilder builder) : base(builder) { }

        public override void Build() {
            Builder.Entity<GamePlayerRating>().HasKey(go => go.Id).ForSqlServerIsClustered(false);
            Builder.Entity<GamePlayerRating>().Property(go => go.Id).UseSqlServerIdentityColumn();
            Builder.Entity<GamePlayerRating>().HasIndex(go => new {
                go.GameId,
                go.PlayerId
            }).IsUnique().ForSqlServerIsClustered();
            Builder.Entity<GamePlayerRating>().Property(gc => gc.GameId).IsRequired();
            Builder.Entity<GamePlayerRating>().Property(gc => gc.PlayerId).IsRequired();
            Builder.Entity<GamePlayerRating>().Property(gc => gc.RatingId).IsRequired();
            Builder.Entity<GamePlayerRating>().HasOne(gc => gc.Game).WithMany(g => g.GamePlayerRatings).HasForeignKey(gc => gc.GameId);
            Builder.Entity<GamePlayerRating>().HasOne(gc => gc.Player).WithMany(c => c.GamePlayerRatings).HasForeignKey(gc => gc.PlayerId);
            Builder.Entity<GamePlayerRating>().HasOne(gc => gc.Rating).WithMany(c => c.GamePlayerRatings).HasForeignKey(gc => gc.RatingId);
        }
    }
}
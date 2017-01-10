using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders {
    public class GameOwnerModelBuilder : BaseModelBuilder {
        public GameOwnerModelBuilder(ModelBuilder builder) : base(builder) { }

        public override void Build() {
            Builder.Entity<GameOwner>().HasKey(go => go.Id).ForSqlServerIsClustered(false);
            Builder.Entity<GameOwner>().Property(go => go.Id).UseSqlServerIdentityColumn();
            Builder.Entity<GameOwner>().HasIndex(go => new {
                go.GameId,
                go.OwnerId
            }).IsUnique().ForSqlServerIsClustered();
            Builder.Entity<GameOwner>().Property(gc => gc.GameId).IsRequired();
            Builder.Entity<GameOwner>().Property(gc => gc.OwnerId).IsRequired();
            Builder.Entity<GameOwner>().HasOne(gc => gc.Game).WithMany(g => g.GameOwners).HasForeignKey(gc => gc.GameId);
            Builder.Entity<GameOwner>().HasOne(gc => gc.Owner).WithMany(c => c.GameOwners).HasForeignKey(gc => gc.OwnerId);
        }
    }
}
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders
{
    public class RatingModelBuilder: BaseModelBuilder
    {
        public RatingModelBuilder(ModelBuilder builder) : base(builder) {}
        public override void Build() {
            Builder.Entity<Rating>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            Builder.Entity<Rating>().Property(c => c.Id).UseSqlServerIdentityColumn();
            Builder.Entity<Rating>().HasIndex(c => c.Rate).ForSqlServerIsClustered().IsUnique();
            Builder.Entity<Rating>().HasIndex(c => c.ShortDescription).IsUnique();
            Builder.Entity<Rating>().Property(c => c.ShortDescription).IsRequired().HasMaxLength(30);
            Builder.Entity<Rating>().Property(c => c.Description).HasMaxLength(100);
        }
    }
}

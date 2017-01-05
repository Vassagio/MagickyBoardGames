using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders
{
    public class CategoryModelBuilder: BaseModelBuilder
    {
        public CategoryModelBuilder(ModelBuilder builder) : base(builder) {}
        public override void Build() {
            Builder.Entity<Category>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            Builder.Entity<Category>().Property(c => c.Id).UseSqlServerIdentityColumn();
            Builder.Entity<Category>().HasIndex(c => c.Description).ForSqlServerIsClustered().IsUnique();
            Builder.Entity<Category>().Property(c => c.Description).IsRequired().HasMaxLength(30);
        }
    }
}

using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders
{
    public class CategoryModelBuilder
    {
        public static void Build(ModelBuilder builder) {
            builder.Entity<Category>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            builder.Entity<Category>().Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<Category>().HasIndex(c => c.Description).ForSqlServerIsClustered().IsUnique();
            builder.Entity<Category>().Property(c => c.Description).IsRequired().HasMaxLength(30);
        }
    }
}

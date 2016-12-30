using MagickyBoardGames.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasKey(c => c.Id).ForSqlServerIsClustered(false);
            builder.Entity<Category>().Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<Category>().HasIndex(c => c.Description).ForSqlServerIsClustered().IsUnique();
        }

        public DbSet<Category> Category { get; set; }
    }
}
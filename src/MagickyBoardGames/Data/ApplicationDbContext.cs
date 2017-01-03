using MagickyBoardGames.Data.ModelBuilders;
using MagickyBoardGames.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            CategoryModelBuilder.Build(builder);
            GameModelBuilder.Build(builder);
        }
    }
}
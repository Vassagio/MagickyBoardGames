using System;
using System.Collections.Generic;
using System.Reflection;
using MagickyBoardGames.Data.ModelBuilders;
using MagickyBoardGames.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace MagickyBoardGames.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<GameOwner> GameOwners { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            new GameModelBuilder(builder).Build();
            new CategoryModelBuilder(builder).Build();
            new GameCategoryModelBuilder(builder).Build();
            new GameOwnerModelBuilder(builder).Build();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
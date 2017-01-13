using System;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MagickyBoardGames.Tests {
    public class DatabaseFixture : IDisposable {
        public ApplicationDbContext Db { get; }

        public DatabaseFixture() {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()   
                .BuildServiceProvider();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);
            Db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public async Task Populate() {
            foreach (var category in Db.Categories)
                Db.Categories.Remove(category);
            await Db.SaveChangesAsync();
            foreach (var game in Db.Games)
                Db.Games.Remove(game);
            await Db.SaveChangesAsync();
            foreach (var rating in Db.Ratings)
                Db.Ratings.Remove(rating);
            await Db.SaveChangesAsync();
            foreach (var user in Db.Users)
                Db.Users.Remove(user);
            await Db.SaveChangesAsync();
            foreach (var gameCategory in Db.GameCategories)
                Db.GameCategories.Remove(gameCategory);
            await Db.SaveChangesAsync();
            foreach (var gameOwner in Db.GameOwners)
                Db.GameOwners.Remove(gameOwner);
            await Db.SaveChangesAsync();
            //foreach (var gamePlayerRating in Db.GamePlayerRatings)
            //    Db.GamePlayerRatings.Remove(gamePlayerRating);
            //await Db.SaveChangesAsync();
        }

        public async Task Populate(params Category[] categories) {
            foreach (var category in Db.Categories)
                Db.Categories.Remove(category);
            await Db.SaveChangesAsync();
            foreach (var category in categories)
                await Db.Categories.AddAsync(category);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params ApplicationUser[] users) {
            foreach (var user in Db.Users)
                Db.Users.Remove(user);
            await Db.SaveChangesAsync();
            foreach (var user in users)
                await Db.Users.AddAsync(user);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params Game[] games) {
            foreach (var game in Db.Games)
                Db.Games.Remove(game);
            await Db.SaveChangesAsync();
            foreach (var game in games)
                await Db.Games.AddAsync(game);
            await Db.SaveChangesAsync();                        
        }


        public async Task Populate(params Rating[] ratings) {
            foreach (var rating in Db.Ratings)
                Db.Ratings.Remove(rating);
            await Db.SaveChangesAsync();
            foreach (var rating in ratings)
                await Db.Ratings.AddAsync(rating);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params GameCategory[] gameCategories) {
            foreach (var gameCategory in Db.GameCategories)
                Db.GameCategories.Remove(gameCategory);
            await Db.SaveChangesAsync();
            foreach (var gameCategory in gameCategories)
                await Db.GameCategories.AddAsync(gameCategory);
            await Db.SaveChangesAsync();
        }

        public async Task Populate(params GameOwner[] gameOwners) {
            foreach (var gameOwner in Db.GameOwners)
                Db.GameOwners.Remove(gameOwner);
            await Db.SaveChangesAsync();
            foreach (var gameOwner in gameOwners)
                await Db.GameOwners.AddAsync(gameOwner);
            await Db.SaveChangesAsync();
        }
        public async Task Populate(params GamePlayerRating[] gamePlayerRatings) {
            //foreach (var gamePlayerRating in Db.GamePlayerRatings)
            //    Db.GamePlayerRatings.Remove(gamePlayerRating);
            //await Db.SaveChangesAsync();
            //foreach (var gamePlayerRating in gamePlayerRatings)
            //    await Db.GamePlayerRatings.AddAsync(gamePlayerRating);
            //await Db.SaveChangesAsync();
        }

        public void Dispose() {
            Db.Dispose();
        }

 
    }
}
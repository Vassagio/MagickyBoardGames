using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MagickyBoardGames.Tests.Repositories {
    public class UserRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public UserRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_User_Repository() {
            var context = BuildUserRepository();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IUserRepository>();
        }

        [Fact]
        public async void Get_All_Records() {
            var user1 = new ApplicationUser {
                Id = "111",
                UserName = "User 1"
            };
            var user2 = new ApplicationUser {
                Id = "222",
                UserName = "User 2"
            };
            var context = BuildUserRepository();
            await _fixture.Populate(user1, user2);

            var categories = await context.GetAll();

            categories.Count().Should().Be(2);
        }

        [Fact]
        public async void Get_By_Id() {
            var user1 = new ApplicationUser {
                Id = "111",
                UserName = "User 1"
            };
            var user2 = new ApplicationUser {
                Id = "222",
                UserName = "User 2"
            };
            var context = BuildUserRepository();
            await _fixture.Populate(user1, user2);

            var user = await context.GetById("222");

            user.Should().Be(user2);
        }

        [Theory]
        [InlineData("User 1")]
        [InlineData("user 1")]
        [InlineData("USER 1")]
        public async void Get_By_Unique_Key(string description) {
            var user1 = new ApplicationUser {
                Id = "111",
                UserName = "User 1"
            };
            var user2 = new ApplicationUser {
                Id = "222",
                UserName = "User 2"
            };
            var context = BuildUserRepository();
            await _fixture.Populate(user1, user2);

            var user = await context.GetBy(description);

            user.Should().Be(user1);
        }

        private UserRepository BuildUserRepository() {
            return new UserRepository(_fixture.Db);
        }
    }
}
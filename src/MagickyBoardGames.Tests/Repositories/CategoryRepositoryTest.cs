using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MagickyBoardGames.Tests.Repositories {
    public class CategoryRepositoryTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public CategoryRepositoryTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_Category_Repository() {
            var context = BuildCategoryRepository();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IRepository<Category>>();
        }

        [Fact]
        public async void Adds_A_Record() {
            var entity = new Category {
                Description = "Added Category"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate();

            var id = await context.Add(entity);

            var expected = await _fixture.Db.Categories.SingleOrDefaultAsync(c => c.Id == id);
            expected.Description.Should().Be(entity.Description);
        }

        [Fact]
        public void Throws_Exception_When_Adding_An_Invalid_Record() {
            var entity = new Category();
            var context = BuildCategoryRepository();

            Func<Task> asyncFunction = async () => { await context.Add(entity); };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Deletes_A_Record() {
            var category = new Category {
                Id = 666,
                Description = "Deleted Category"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category);

            await context.Delete(666);

            _fixture.Db.Categories.Any().Should().BeFalse();
        }

        [Fact]
        public void Does_Not_Throw_Exception_When_Deleting_An_Unknown_Record() {
            var context = BuildCategoryRepository();

            Func<Task> asyncFunction = async () => { await context.Delete(100); };
            asyncFunction.ShouldNotThrow();
        }

        [Fact]
        public async void Updates_A_Record() {
            var category = new Category {
                Id = 999,
                Description = "Original Category"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category);
            category.Description = "Updated Category";

            await context.Update(category);

            var expected = _fixture.Db.Categories.SingleOrDefault(g => g.Id == 999);
            expected.Description.Should().Be("Updated Category");
        }

        [Fact]
        public async void Throws_Exception_When_Updating_With_Invalid_Record() {
            var category = new Category {
                Id = 9991,
                Description = "Original Category"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category);

            Func<Task> asyncFunction = async () => {
                category.Description = "";
                await context.Update(category);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_Exception_When_Updating_A_Record_That_Doesnt_Exists() {
            var context = BuildCategoryRepository();

            Func<Task> asyncFunction = async () => {
                var updated = new Category {
                    Id = 1000,
                    Description = "Doesn't Exist"
                };
                await context.Update(updated);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Get_All_Records() {
            var category1 = new Category {
                Id = 111,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 222,
                Description = "Category 2"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category1, category2);

            var categories = await context.GetAll();

            categories.Count().Should().Be(2);
        }

        [Fact]
        public async void Get_By_Id() {
            var category1 = new Category {
                Id = 111,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 222,
                Description = "Category 2"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category1, category2);

            var category = await context.GetBy(222);

            category.Should().Be(category2);
        }

        [Theory]
        [InlineData("Category 1")]
        [InlineData("category 1")]
        [InlineData("CATEGORY 1")]
        public async void Get_By_Unique_Key(string description) {
            var category1 = new Category {
                Id = 111,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 222,
                Description = "Category 2"
            };
            var context = BuildCategoryRepository();
            await _fixture.Populate(category1, category2);

            var category = await context.GetBy(description);

            category.Should().Be(category1);
        }

        private CategoryRepository BuildCategoryRepository() {
            return new CategoryRepository(_fixture.Db);
        }
    }
}
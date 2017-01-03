using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts {
    public class CategoryContextTest : IClassFixture<DatabaseFixture> {
        private readonly DatabaseFixture _fixture;

        public CategoryContextTest(DatabaseFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_A_Category_Context() {
            var context = BuildCategoryContext();

            context.Should().NotBeNull();
            context.Should().BeAssignableTo<IContext<CategoryViewModel>>();
        }

        [Fact]
        public async void Adds_A_Record() {
            var viewModel = new CategoryViewModel {
                Description = "Added Category"
            };
            var context = BuildCategoryContext();
            await _fixture.Populate();

            var id = await context.Add(viewModel);

            var expected = await _fixture.Db.Categories.SingleOrDefaultAsync(c => c.Id == id);
            expected.Description.Should().Be(viewModel.Description);
        }

        [Fact]
        public void Throws_Exception_When_Adding_An_Invalid_Record() {
            var viewModel = new CategoryViewModel();
            var context = BuildCategoryContext();

            Func<Task> asyncFunction = async () => { await context.Add(viewModel); };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async void Deletes_A_Record() {
            var category = new Category {
                Id = 1,
                Description = "Deleted Category"
            };
            var context = BuildCategoryContext();
            await _fixture.Populate(category);

            await context.Delete(1);

            _fixture.Db.Categories.Any().Should().BeFalse();
        }

        [Fact]
        public void Does_Not_Throw_Exception_When_Deleting_An_Unknown_Record() {
            var context = BuildCategoryContext();

            Func<Task> asyncFunction = async () => { await context.Delete(100); };
            asyncFunction.ShouldNotThrow();
        }

        [Fact]
        public async void Updates_A_Record() {
            var category = new Category {
                Id = 1,
                Description = "Original Category"
            };
            var context = BuildCategoryContext();
            await _fixture.Populate(category);
            var updated = new CategoryViewModel {
                Id = 1,
                Description = "Updated Category"
            };

            await context.Update(updated);

            var expected = _fixture.Db.Categories.SingleOrDefault(g => g.Id == 1);
            expected.Description.Should().Be("Updated Category");
        }

        [Fact]
        public async void Throws_Exception_When_Updating_With_Invalid_Record() {
            var category = new Category {
                Id = 10,
                Description = "Original Category"
            };
            var context = BuildCategoryContext();
            await _fixture.Populate(category);

            Func<Task> asyncFunction = async () => {
                var updated = new CategoryViewModel {
                    Id = 10,
                    Description = ""
                };
                await context.Update(updated);
            };
            asyncFunction.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_Exception_When_Updating_A_Record_That_Doesnt_Exists() {
            var context = BuildCategoryContext();

            Func<Task> asyncFunction = async () => {
                var updated = new CategoryViewModel {
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
                Id = 1,
                Description = "Category 1"
            };
            var category2 = new Category {
                Id = 2,
                Description = "Category 2"
            };
            var context = BuildCategoryContext();
            await _fixture.Populate(category1, category2);

            var categories = await context.GetAll();

            categories.Count().Should().Be(2);
        }

        private CategoryContext BuildCategoryContext() {
            return new CategoryContext(_fixture.Db);
        }
    }
}
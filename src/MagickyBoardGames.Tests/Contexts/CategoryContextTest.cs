using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Data;
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

            var id = await context.Add(viewModel);

            var expected = await context.GetBy(id);
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
            var viewModel = new CategoryViewModel {
                Description = "Deleted Category"
            };
            var context = BuildCategoryContext();

            var id = await context.Add(viewModel);
            await context.Delete(id);

            var expected = await context.GetBy(id);
            expected.Should().BeNull();
        }

        [Fact]
        public void Does_Not_Throw_Exception_When_Deleting_An_Unknown_Record() {
            var context = BuildCategoryContext();

            Func<Task> asyncFunction = async () => { await context.Delete(100); };
            asyncFunction.ShouldNotThrow();
        }

        [Fact]
        public async void Updates_A_Record() {
            var viewModel = new CategoryViewModel {
                Description = "Original Category"
            };
            var context = BuildCategoryContext();

            var id = await context.Add(viewModel);
            var updated = await context.GetBy(id);
            updated.Description = "Updated Category";

            await context.Update(updated);

            var expected = await context.GetBy(id);
            expected.Description.Should().Be("Updated Category");
        }

        [Fact]
        public void Throws_Exception_When_Updating_With_Invalid_Record() {
            var viewModel = new CategoryViewModel {
                Description = "Original Category"
            };
            var context = BuildCategoryContext();

            Func<Task> asyncFunction = async () => {
                var id = await context.Add(viewModel);
                var updated = await context.GetBy(id);
                updated.Description = "";
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
            var category1 = new CategoryViewModel {
                Description = "Category 1"
            };
            var category2 = new CategoryViewModel {
                Description = "Category 2"
            };
            var context = BuildCategoryContext();
            var categories = await context.GetAll();
            var originalCount = categories.Count();

            await context.Add(category1);
            await context.Add(category2);

            categories = await context.GetAll();

            categories.Count().Should().Be(originalCount + 2);
        }

        private CategoryContext BuildCategoryContext() {
            return new CategoryContext(_fixture.Db);
        }
    }
}
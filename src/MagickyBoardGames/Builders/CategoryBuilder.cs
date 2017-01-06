using System;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardCategorys.Builders
{
    public class CategoryBuilder : IBuilder<Category, CategoryViewModel> {
        private int? _id;
        private string _description;

        public CategoryViewModel ToViewModel() {
            return new CategoryViewModel {
                Id = _id,
                Description = _description,
            };
        }

        public Category ToEntity() {
            if (!_id.HasValue)
                throw new ArgumentException();

            return new Category {
                Id = _id.Value,
                Description = _description,
            };
        }

        public CategoryViewModel Build(Category entity) {
            _id = entity.Id;
            _description = entity.Description;
            return ToViewModel();
        }

        public Category Build(CategoryViewModel viewModel) {
            _id = viewModel.Id;
            _description = viewModel.Description;
            return ToEntity();
        }
    }
}

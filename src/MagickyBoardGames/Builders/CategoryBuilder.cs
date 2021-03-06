﻿using System;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders
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
            var entity =  new Category {                
                Description = _description,
            };
            if (_id.HasValue)
                entity.Id = _id.Value;
            return entity;
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

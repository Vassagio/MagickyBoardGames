﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Contexts {
    public class CategoryContext : IContext<CategoryViewModel> {
        private readonly ApplicationDbContext _context;

        public CategoryContext(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAll() {
            return await _context.Categories.Select(category => new CategoryViewModel {
                Id = category.Id,
                Description = category.Description
            }).ToListAsync();
        }

        public async Task<int> Add(CategoryViewModel viewModel) {
            if (string.IsNullOrEmpty(viewModel.Description))
                throw new ArgumentException();

            var category = new Category {
                Description = viewModel.Description
            };
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (category != null)
                _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryViewModel> GetBy(int id) {
            return await _context.Categories.Select(category => new CategoryViewModel {
                Id = category.Id,
                Description = category.Description
            }).SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(CategoryViewModel viewModel) {
            if (string.IsNullOrEmpty(viewModel.Description))
                throw new ArgumentException();

            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == viewModel.Id);
            if (category == null)
                throw new ArgumentException();

            category.Description = viewModel.Description;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
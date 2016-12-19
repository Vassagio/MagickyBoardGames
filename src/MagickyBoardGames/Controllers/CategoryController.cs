using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryViewModel.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryViewModel = await _context.CategoryViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryViewModel = await _context.CategoryViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (categoryViewModel == null)
            {
                return NotFound();
            }
            return View(categoryViewModel);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] CategoryViewModel categoryViewModel)
        {
            if (id != categoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryViewModelExists(categoryViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryViewModel = await _context.CategoryViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryViewModel = await _context.CategoryViewModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.CategoryViewModel.Remove(categoryViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoryViewModelExists(int id)
        {
            return _context.CategoryViewModel.Any(e => e.Id == id);
        }
    }
}

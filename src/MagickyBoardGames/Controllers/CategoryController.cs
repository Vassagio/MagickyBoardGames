using System.Threading.Tasks;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class CategoryController : Controller {
        private readonly ICategoryContext _categoryContext;

        public CategoryController(ICategoryContext categoryContext) {
            _categoryContext = categoryContext;
        }

        public async Task<IActionResult> Index() {
            return View(await _categoryContext.GetAll());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();           

            var categoryViewModel = await _categoryContext.GetBy(id.Value);
            if (categoryViewModel == null) 
                return NotFound();            

            return View(categoryViewModel);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] CategoryViewModel categoryViewModel) {
            if (!ModelState.IsValid)
                return View(categoryViewModel);

            await _categoryContext.Add(categoryViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) 
                return NotFound();            

            var categoryViewModel = await _categoryContext.GetBy(id.Value);
            if (categoryViewModel == null) 
                return NotFound();            
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] CategoryViewModel categoryViewModel) {
            if (id != categoryViewModel.Id) 
                return NotFound();

            if (!ModelState.IsValid)
                return View(categoryViewModel);

            await _categoryContext.Update(categoryViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) 
                return NotFound();            

            var categoryViewModel = await _categoryContext.GetBy(id.Value);
            if (categoryViewModel == null) 
                return NotFound();            

            return View(categoryViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _categoryContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
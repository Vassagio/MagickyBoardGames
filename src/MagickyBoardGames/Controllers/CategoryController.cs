using System.Threading.Tasks;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class CategoryController : Controller {
        private readonly IContextLoader _loader;

        public CategoryController(IContextLoader loader) {
            _loader = loader;
        }

        public async Task<IActionResult> Index() {
            var context = _loader.LoadCategoryListContext();
            return View(await context.BuildViewModel());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadCategoryViewContext();
            var viewModel = await context.BuildViewModel(id.Value);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Description")] CategoryViewModel categoryViewModel) {
            var context = _loader.LoadCategorySaveContext();
            var result = context.Validate(categoryViewModel);
            if (!result.IsValid)
                return View(categoryViewModel);

            await context.Save(categoryViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadCategoryViewContext();
            var categoryViewViewModel = await context.BuildViewModel(id.Value);
            if (categoryViewViewModel == null)
                return NotFound();

            return View(categoryViewViewModel.Category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] CategoryViewModel categoryViewModel) {
            if (id != categoryViewModel.Id)
                return NotFound();

            var context = _loader.LoadCategorySaveContext();
            var result = context.Validate(categoryViewModel);
            if (!result.IsValid)            
                return View(categoryViewModel);

            await context.Save(categoryViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadCategoryViewContext();
            var viewModel = await context.BuildViewModel(id.Value);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var context = _loader.LoadCategoryViewContext();
            await context.Delete(id);
            return RedirectToAction("Index");
        }

        private bool IsValid(CategoryViewModel categoryViewModel) {
            //var results = _validator.Validate(categoryViewModel);
            //results.AddToModelState(ModelState, null);
            //return results.IsValid;
            return true;
        }
    }
}
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
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
            var context = _loader.LoadCategoryIndexContext();
            return View(await context.BuildViewModel());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadCategoryDetailContext();
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
            //if (!IsValid(categoryViewModel))
            //    return View(categoryViewModel);

            //await _categoryContext.Add(categoryViewModel);
            //return RedirectToAction("Index");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            //if (id == null)
            //    return NotFound();

            //var categoryViewModel = await _categoryContext.GetBy(id.Value);
            //if (categoryViewModel == null)
            //    return NotFound();
            //return View(categoryViewModel);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] CategoryViewModel categoryViewModel) {
            //if (id != categoryViewModel.Id)
            //    return NotFound();

            //if (!IsValid(categoryViewModel))
            //    return View(categoryViewModel);

            //await _categoryContext.Update(categoryViewModel);
            //return RedirectToAction("Index");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            //if (id == null)
            //    return NotFound();

            //var categoryViewModel = await _categoryContext.GetBy(id.Value);
            //if (categoryViewModel == null)
            //    return NotFound();

            //return View(categoryViewModel);
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            //await _categoryContext.Delete(id);
            //return RedirectToAction("Index");
            return View();
        }   

        private bool IsValid(CategoryViewModel categoryViewModel) {
            //var results = _validator.Validate(categoryViewModel);
            //results.AddToModelState(ModelState, null);
            //return results.IsValid;
            return true;
        }
    }
}
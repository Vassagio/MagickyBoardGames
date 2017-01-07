using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class GameController : Controller {
        private readonly IGameContextLoader _loader;

        public GameController(IGameContextLoader loader) {
            _loader = loader;
        }

        public async Task<IActionResult> Index() {
            var context = _loader.LoadGameListContext();
            return View(await context.BuildViewModel());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadGameViewContext();
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MinPlayers,MaxPlayers")] GameViewModel gameViewModel) {
            var context = _loader.LoadGameSaveContext();
            var result = context.Validate(gameViewModel);
            if (!result.IsValid)
                return View(gameViewModel);

            await context.Save(gameViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadGameViewContext();
            var gameViewViewModel = await context.BuildViewModel(id.Value);
            if (gameViewViewModel == null)
                return NotFound();

            return View(gameViewViewModel.Game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MinPlayers,MaxPlayers")] GameViewModel gameViewModel) {
            if (id != gameViewModel.Id)
                return NotFound();

            var context = _loader.LoadGameSaveContext();
            var result = context.Validate(gameViewModel);
            if (!result.IsValid)
                return View(gameViewModel);

            await context.Save(gameViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadGameViewContext();
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
            var context = _loader.LoadGameViewContext();
            await context.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
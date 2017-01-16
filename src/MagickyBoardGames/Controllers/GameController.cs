using System.Threading.Tasks;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Services;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class GameController : Controller {
        private readonly IGameContextLoader _loader;
        private readonly IApplicationUserManager _applicationUserManager;

        public GameController(IGameContextLoader loader, IApplicationUserManager applicationUserManager) {
            _loader = loader;
            _applicationUserManager = applicationUserManager;
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
        public async Task<IActionResult> Create() {
            var context = _loader.LoadGameSaveContext();
            var userId = _applicationUserManager.GetUserId(HttpContext.User);
            var viewModel = await context.BuildViewModel();
            viewModel.UserId = userId;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Game,UserId,CategoryIds,OwnerIds,RatingId")] GameSaveViewModel gameSaveViewModel) {
            var context = _loader.LoadGameSaveContext();
            var result = context.Validate(gameSaveViewModel);
            if (!result.IsValid) {
                var viewModel = await context.BuildViewModel();
                gameSaveViewModel.AvailableCategories = viewModel.AvailableCategories;
                gameSaveViewModel.AvailableOwners = viewModel.AvailableOwners;
                gameSaveViewModel.AvailableRatings = viewModel.AvailableRatings;
                return View(gameSaveViewModel);
            }

            await context.Save(gameSaveViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadGameSaveContext();
            var userId = _applicationUserManager.GetUserId(HttpContext.User);
            var viewModel = await context.BuildViewModel(id.Value, userId);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Game,UserId,CategoryIds,OwnerIds,RatingId")] GameSaveViewModel gameSaveViewModel) {
            if (id != gameSaveViewModel.Game.Id)
                return NotFound();

            var context = _loader.LoadGameSaveContext();
            var result = context.Validate(gameSaveViewModel);
            if (!result.IsValid) {
                var viewModel = await context.BuildViewModel();
                gameSaveViewModel.AvailableCategories = viewModel.AvailableCategories;
                gameSaveViewModel.AvailableOwners = viewModel.AvailableOwners;
                gameSaveViewModel.AvailableRatings = viewModel.AvailableRatings;
                return View(gameSaveViewModel);
            }

            await context.Save(gameSaveViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Rate(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadGameRateContext();
            var userId = _applicationUserManager.GetUserId(HttpContext.User);
            var viewModel = await context.BuildViewModel(id.Value, userId);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rate(int id, [Bind("Game,UserId,RatingId")] GameRateViewModel gameRateViewModel) {
            if (id != gameRateViewModel.Game.Id)
                return NotFound();

            var context = _loader.LoadGameRateContext();            
            await context.Save(gameRateViewModel);
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
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class GameController : Controller {
        private readonly IContext<GameViewModel> _gameContext;
        private readonly IValidator<GameViewModel> _validator;

        public GameController(IContext<GameViewModel> gameContext, IValidator<GameViewModel> validator) {
            _gameContext = gameContext;
            _validator = validator;
        }

        public async Task<IActionResult> Index() {
            return View(await _gameContext.GetAll());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var gameViewModel = await _gameContext.GetBy(id.Value);
            if (gameViewModel == null)
                return NotFound();

            return View(gameViewModel);
        }

        [Authorize]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MinPlayers,MaxPlayers")] GameViewModel gameViewModel) {
            if (!IsValid(gameViewModel))
                return View(gameViewModel);

            await _gameContext.Add(gameViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var gameViewModel = await _gameContext.GetBy(id.Value);
            if (gameViewModel == null)
                return NotFound();
            return View(gameViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MinPlayers,MaxPlayers")] GameViewModel gameViewModel) {
            if (id != gameViewModel.Id)
                return NotFound();

            if (!IsValid(gameViewModel))
                return View(gameViewModel);

            await _gameContext.Update(gameViewModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
                return NotFound();

            var gameViewModel = await _gameContext.GetBy(id.Value);
            if (gameViewModel == null)
                return NotFound();

            return View(gameViewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _gameContext.Delete(id);
            return RedirectToAction("Index");
        }

        private bool IsValid(GameViewModel gameViewModel) {
            var results = _validator.Validate(gameViewModel);
            results.AddToModelState(ModelState, null);
            return results.IsValid;
        }
    }
}
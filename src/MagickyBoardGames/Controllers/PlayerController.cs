using System.Threading.Tasks;
using MagickyBoardGames.Contexts.PlayerContexts;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class PlayerController : Controller {
        private readonly IPlayerContextLoader _loader;

        public PlayerController(IPlayerContextLoader loader) {
            _loader = loader;
        }

        public async Task<IActionResult> Index() {
            var context = _loader.LoadPlayerListContext();
            return View(await context.BuildViewModel());
        }

        public async Task<IActionResult> Details(string id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadPlayerViewContext();
            var viewModel = await context.BuildViewModel(id);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }
    }
}
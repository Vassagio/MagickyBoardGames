using System.Threading.Tasks;
using MagickyBoardGames.Contexts.RatingContexts;
using Microsoft.AspNetCore.Mvc;

namespace MagickyBoardGames.Controllers {
    public class RatingController : Controller {
        private readonly IRatingContextLoader _loader;

        public RatingController(IRatingContextLoader loader) {
            _loader = loader;
        }

        public async Task<IActionResult> Index() {
            var context = _loader.LoadRatingListContext();
            return View(await context.BuildViewModel());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var context = _loader.LoadRatingViewContext();
            var viewModel = await context.BuildViewModel(id.Value);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }
    }
}
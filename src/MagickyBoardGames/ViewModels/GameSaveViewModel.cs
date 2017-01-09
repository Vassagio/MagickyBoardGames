using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels {
    public class GameSaveViewModel {
        public GameViewModel Game { get; set; }
        public IEnumerable<CategoryViewModel> SelectedCategories { get; set; } = new List<CategoryViewModel>();
    }
}
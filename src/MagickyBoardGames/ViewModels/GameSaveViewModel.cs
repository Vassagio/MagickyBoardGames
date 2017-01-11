using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels {
    public class GameSaveViewModel: IViewModel {
        public GameViewModel Game { get; set; }
        public int[] CategoryIds { get; set; } = new int[0];
        public IEnumerable<CategoryViewModel> AvailableCategories { get; set; } = new List<CategoryViewModel>();
        public string[] OwnerIds { get; set; } = new string[0];
        public IEnumerable<OwnerViewModel> AvailableOwners { get; set; } = new List<OwnerViewModel>();
    }
}
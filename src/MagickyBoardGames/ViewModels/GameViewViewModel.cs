using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class GameViewViewModel
    {
        public GameViewModel Game { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<OwnerViewModel> Owners { get; set; } = new List<OwnerViewModel>();
    }
}

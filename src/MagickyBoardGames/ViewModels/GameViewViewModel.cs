using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class GameViewViewModel
    {
        public GameViewModel Game { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}

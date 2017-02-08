using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class GameListViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }
        public GameFilterViewModel Filter { get; set; } = new GameFilterViewModel();
    }
}

using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class RatingViewViewModel
    {
        public RatingViewModel Rating { get; set; }
        public IEnumerable<GameViewModel> Games { get; set; }
    }
}

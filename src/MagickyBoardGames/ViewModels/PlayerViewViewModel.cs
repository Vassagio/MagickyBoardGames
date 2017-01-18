using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class PlayerViewViewModel
    {
        public PlayerViewModel Player { get; set; }
        public IEnumerable<GameViewModel> Games { get; set; }
        public IEnumerable<GameRatingViewModel> GamesRated { get; set; }
    }
}

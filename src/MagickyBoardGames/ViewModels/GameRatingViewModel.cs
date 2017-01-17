using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels {
    public class GameRatingViewModel : IViewModel {
        [Display(Name = "Game Name")]
        public string GameName { get; set; }

        public RatingViewModel Rating { get; set; }
    }
}
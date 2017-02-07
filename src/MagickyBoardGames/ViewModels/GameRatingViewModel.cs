using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels {
    public class GameRatingViewModel : IViewModel {
        public int Id { get; set; }
        [Display(Name = "Game Name")]
        public string GameName { get; set; }

        public RatingViewModel Rating { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels {
    public class GameRateViewModel: IViewModel {
        public GameViewModel Game { get; set; }
        public string UserId { get; set; }        
        public int RatingId { get; set; }
        public IEnumerable<RatingViewModel> AvailableRatings { get; set; } = new List<RatingViewModel>();
        [Display(Name = "Player Ratings")]
        public IEnumerable<PlayerRatingViewModel> PlayerRatings { get; set; } = new List<PlayerRatingViewModel>();
    }
}
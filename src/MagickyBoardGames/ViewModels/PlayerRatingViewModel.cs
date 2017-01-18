using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels
{
    public class PlayerRatingViewModel : IViewModel
    {
        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        public RatingViewModel Rating {get; set;}
    }
}

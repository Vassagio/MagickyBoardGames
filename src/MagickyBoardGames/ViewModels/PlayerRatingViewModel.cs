using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels
{
    public class PlayerRatingViewModel : IViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        public RatingViewModel Rating {get; set;}
    }
}

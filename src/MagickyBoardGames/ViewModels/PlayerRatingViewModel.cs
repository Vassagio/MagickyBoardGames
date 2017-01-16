namespace MagickyBoardGames.ViewModels
{
    public class PlayerRatingViewModel : IViewModel
    {
        public string PlayerName { get; set; }
        public RatingViewModel Rating {get; set;}
    }
}

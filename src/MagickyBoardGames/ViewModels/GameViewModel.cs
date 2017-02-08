using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels
{
    public class GameViewModel : IViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [Display(Name = "Minimum Number Of Players")]
        public int? MinPlayers { get; set; }
        [Display(Name = "Maximum Number Of Players")]
        public int? MaxPlayers { get; set; }
        [Display(Name = "# Of Players")]
        public string PlayerRange { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public int GameId { get; set; }
    }
}

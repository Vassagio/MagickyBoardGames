using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels
{
    public class GameViewModel : IViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Minimum Number Of Players")]
        public int? MinPlayers { get; set; }
        [Display(Name = "Maximum Number Of Players")]
        public int? MaxPlayers { get; set; }
        [Display(Name = "Number Of Players")]
        public string PlayerRange { get; set; }
    }
}

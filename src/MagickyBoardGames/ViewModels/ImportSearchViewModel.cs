using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels
{
    public class ImportSearchViewModel
    {
        [Display(Name = "Search")]
        public string Query { get; set; }

        public IEnumerable<GameViewModel> BoardGames { get; set; } = new List<GameViewModel>();
    }
}

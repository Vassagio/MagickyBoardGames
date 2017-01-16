using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels {
    public class GameSaveViewModel: IViewModel {
        public GameViewModel Game { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Categories")]
        public int[] CategoryIds { get; set; } = new int[0];
        public IEnumerable<CategoryViewModel> AvailableCategories { get; set; } = new List<CategoryViewModel>();
        [Display(Name = "Owners")]
        public string[] OwnerIds { get; set; } = new string[0];
        public IEnumerable<OwnerViewModel> AvailableOwners { get; set; } = new List<OwnerViewModel>();
        [Display(Name = "Rating")]
        public int RatingId { get; set; }
        public IEnumerable<RatingViewModel> AvailableRatings { get; set; } = new List<RatingViewModel>();
    }
}
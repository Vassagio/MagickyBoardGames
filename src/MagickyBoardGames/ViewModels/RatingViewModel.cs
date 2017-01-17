using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.ViewModels {
    public class RatingViewModel : IViewModel {
        public int? Id { get; set; }
        public int Rate { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
    }
}
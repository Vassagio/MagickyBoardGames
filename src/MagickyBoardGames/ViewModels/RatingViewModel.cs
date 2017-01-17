namespace MagickyBoardGames.ViewModels {
    public class RatingViewModel : IViewModel {
        public int? Id { get; set; }
        public int Rate { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
    }
}
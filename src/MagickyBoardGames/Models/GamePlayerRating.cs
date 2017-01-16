namespace MagickyBoardGames.Models {
    public class GamePlayerRating : IEntity {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string PlayerId { get; set; }
        public ApplicationUser Player { get; set; }
        public int RatingId { get; set; }
        public Rating Rating { get; set; }
    }
}
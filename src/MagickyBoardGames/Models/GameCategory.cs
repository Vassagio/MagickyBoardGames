using System.ComponentModel.DataAnnotations.Schema;

namespace MagickyBoardGames.Models {
    public class GameCategory: IEntity {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
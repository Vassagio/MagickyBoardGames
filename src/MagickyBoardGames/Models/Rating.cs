using System.Collections.Generic;

namespace MagickyBoardGames.Models
{
    public class Rating : IEntity
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public virtual ICollection<GamePlayerRating> GamePlayerRatings { get; set; }
    }
}

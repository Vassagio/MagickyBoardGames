using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagickyBoardGames.Models {
    public class Category: IEntity {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<GameCategory> GameCategories { get; set; }
    }
}
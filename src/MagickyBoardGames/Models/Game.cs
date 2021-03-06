﻿using System;
using System.Collections.Generic;

namespace MagickyBoardGames.Models {
    public class Game : IEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<GameCategory> GameCategories { get; set; }
        public virtual ICollection<GameOwner> GameOwners { get; set; }
        public virtual ICollection<GamePlayerRating> GamePlayerRatings { get; set; }
    }
}
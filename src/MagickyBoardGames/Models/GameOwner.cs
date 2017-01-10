using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagickyBoardGames.Models
{
    public class GameOwner
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}

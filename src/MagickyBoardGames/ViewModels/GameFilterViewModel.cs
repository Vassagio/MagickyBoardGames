using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagickyBoardGames.ViewModels
{
    public class GameFilterViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NumberOfPlayers { get; set; }
        public int? Rating { get; set; }
    }
}

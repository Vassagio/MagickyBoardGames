using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagickyBoardGames.ViewModels
{
    public class GameViewModel : IViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string PlayerRange { get; set; }
    }
}

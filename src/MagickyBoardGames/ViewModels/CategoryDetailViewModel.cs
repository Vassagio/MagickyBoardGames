﻿using System.Collections.Generic;

namespace MagickyBoardGames.ViewModels
{
    public class CategoryDetailViewModel
    {
        public CategoryViewModel Category { get; set; }
        public IEnumerable<GameViewModel> Games { get; set; }
    }
}

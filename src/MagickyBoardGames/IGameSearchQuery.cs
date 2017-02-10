using System.Collections.Generic;
using MagickyBoardGames.Models;

namespace MagickyBoardGames {
    //TODO: Needs Mocking
    public interface IGameSearchQuery {
        IGameSearchQuery FilterByName(string name);
        IGameSearchQuery FilterByDescription(string description);
        IGameSearchQuery FilterByNumberOfPlayers(int? numberOfPlayers);
        IGameSearchQuery FilterByAverageRating(int? averageRating);
        IEnumerable<Game> Execute();
        IGameSearchQuery Limit(int count);
        IGameSearchQuery Offset(int count);
        int Count();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.Services.BoardGameGeek;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Services {
    public interface IGameInfoService {
        Task<IEnumerable<SearchResult>> Search(string query);
        Task<GameViewModel> LoadGame(int gameId);
        Task<IEnumerable<GameViewModel>> LoadGames(params int[] gameIds);
    }
}
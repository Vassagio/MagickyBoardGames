using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts
{
    public interface IGameListContext
    {
        Task<GameListViewModel> BuildViewModel();
    }
}

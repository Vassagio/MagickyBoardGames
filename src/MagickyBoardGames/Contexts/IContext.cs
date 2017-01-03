using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts {
    public interface IContext<T> where T: IViewModel {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetBy(int id);
        Task<int> Add(T viewModel);
        Task Delete(int id);
        Task Update(T viewModel);
    }
}
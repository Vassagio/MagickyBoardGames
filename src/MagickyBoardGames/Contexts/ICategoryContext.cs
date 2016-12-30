using System.Collections.Generic;
using System.Threading.Tasks;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts {
    public interface ICategoryContext {
        Task<IEnumerable<CategoryViewModel>> GetAll();
        Task<CategoryViewModel> GetBy(int id);
        Task<int> Add(CategoryViewModel categoryViewModel);
        Task Delete(int id);
        Task Update(CategoryViewModel categoryViewModel);
    }
}
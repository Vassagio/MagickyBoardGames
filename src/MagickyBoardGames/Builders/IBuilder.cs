using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public interface IBuilder<TEntity, TViewModel> where TEntity : IEntity where TViewModel : IViewModel {
        TViewModel ToViewModel();
        TEntity ToEntity();
        TViewModel Build(TEntity entity);
        TEntity Build(TViewModel viewModel);
    }
}

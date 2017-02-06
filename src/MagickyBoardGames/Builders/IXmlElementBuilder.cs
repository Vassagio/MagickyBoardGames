using System.Xml.Linq;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public interface IXmlElementBuilder<out TViewModel> where TViewModel : IViewModel {
        TViewModel ToViewModel();
        TViewModel Build(XElement element);
    }
}
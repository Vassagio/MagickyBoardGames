using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public class OwnerBuilder : IBuilder<ApplicationUser, OwnerViewModel> {
        private string _id;
        private string _name;

        public OwnerViewModel ToViewModel() {
            return new OwnerViewModel {
                Id = _id,
                Name = _name
            };
        }

        public ApplicationUser ToEntity() {
            return new ApplicationUser {
                Id = _id,
                UserName = _name
            };
        }

        public OwnerViewModel Build(ApplicationUser entity) {
            _id = entity.Id;
            _name = entity.UserName;
            return ToViewModel();
        }

        public ApplicationUser Build(OwnerViewModel viewModel) {
            _id = viewModel.Id;
            _name = viewModel.Name;
            return ToEntity();
        }
    }
}
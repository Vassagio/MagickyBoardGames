using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public class PlayerBuilder : IBuilder<ApplicationUser, PlayerViewModel> {
        private string _id;
        private string _name;
        private string _email;

        public PlayerViewModel ToViewModel() {
            return new PlayerViewModel {
                Id = _id,
                Name = _name,
                Email = _email
            };
        }

        public ApplicationUser ToEntity() {
            return new ApplicationUser {
                Id = _id,
                UserName = _name,
                Email = _email
            };
        }

        public PlayerViewModel Build(ApplicationUser entity) {
            _id = entity.Id;
            _name = entity.UserName;
            _email = entity.Email;
            return ToViewModel();
        }

        public ApplicationUser Build(PlayerViewModel viewModel) {
            _id = viewModel.Id;
            _name = viewModel.Name;
            _email = viewModel.Email;
            return ToEntity();
        }
    }
}
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders {
    public class PlayerRatingBuilder : IBuilder<GamePlayerRating, PlayerRatingViewModel> {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;
        private readonly IUserRepository _userRepository;
        private string _id;
        private string _playerName;
        private RatingViewModel _rating;

        public PlayerRatingBuilder(IRatingRepository ratingRepository, IBuilder<Rating, RatingViewModel> ratingBuilder, IUserRepository userRepository) {
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
            _userRepository = userRepository;
        }

        public PlayerRatingViewModel ToViewModel() {
            return new PlayerRatingViewModel {
                Id = _id,
                PlayerName = _playerName,
                Rating = _rating
            };
        }

        public PlayerRatingViewModel Build(GamePlayerRating entity) {
            var player = _userRepository.GetById(entity.PlayerId).Result;
            var rating = _ratingRepository.GetBy(entity.RatingId).Result;
            if (player == null || rating == null)
                return new PlayerRatingViewModel();
            _id = player.Id;
            _playerName = player.UserName;
            _rating = _ratingBuilder.Build(rating);
            return ToViewModel();
        }

        public GamePlayerRating ToEntity() {
            return new GamePlayerRating();
        }

        public GamePlayerRating Build(PlayerRatingViewModel viewModel) {
            return ToEntity();
        }
    }
}
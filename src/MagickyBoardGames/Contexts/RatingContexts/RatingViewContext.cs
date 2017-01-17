using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.RatingContexts
{
    public class RatingViewContext : IRatingViewContext {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;

        public RatingViewContext(IRatingRepository ratingRepository, IBuilder<Rating, RatingViewModel> ratingBuilder, IBuilder<Game, GameViewModel> gameBuilder) {
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
            _gameBuilder = gameBuilder;
        }

        public async Task<RatingViewViewModel> BuildViewModel(int id) {
            var rating = await _ratingRepository.GetBy(id);
            if (rating == null)
                return new RatingViewViewModel();

            return new RatingViewViewModel {
                Rating = _ratingBuilder.Build(rating),
                Games = GetGames(rating) ?? new List<GameViewModel>()
            };
        }

        private IEnumerable<GameViewModel> GetGames(Rating rating) {
            return rating.GamePlayerRatings?.Select(go => _gameBuilder.Build(go.Game)).ToList();
        }
    }
}

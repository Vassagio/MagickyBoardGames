using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.RatingContexts
{
    public class RatingListContext : IRatingListContext
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;

        public RatingListContext(IRatingRepository ratingRepository, IBuilder<Rating, RatingViewModel> ratingBuilder) {
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
        }

        public async Task<RatingListViewModel> BuildViewModel() {
            var players = await _ratingRepository.GetAll();

            var viewModels = players.Select(rating => _ratingBuilder.Build(rating)).ToList();
            return new RatingListViewModel {
                Ratings = viewModels
            };
        }
    }
}

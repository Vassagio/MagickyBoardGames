using System;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders
{
    public class RatingBuilder : IBuilder<Rating, RatingViewModel> {
        private static readonly string SEPARATOR = " - ";
        private int? _id;
        private int _rate;
        private string _shortDescription;
        private string _description;        

        public RatingViewModel ToViewModel() {
            return new RatingViewModel {
                Id = _id,
                Rate = _rate,
                ShortDescription = _shortDescription,
                Description = _description,
                LongDescription = BuildLongDescription()
            };
        }

        public Rating ToEntity() {                        
            var entity =  new Rating {     
                Rate =  _rate,
                ShortDescription = _shortDescription,           
                Description = _description,
            };
            if (_id.HasValue)
                entity.Id = _id.Value;
            return entity;
        }

        public RatingViewModel Build(Rating entity) {
            _id = entity.Id;
            _rate = entity.Rate;
            _shortDescription = entity.ShortDescription;
            _description = entity.Description;
            return ToViewModel();
        }

        public Rating Build(RatingViewModel viewModel) {
            _id = viewModel.Id;
            _rate = viewModel.Rate;
            _shortDescription = viewModel.ShortDescription;
            _description = viewModel.Description;
            return ToEntity();
        }

        private string BuildLongDescription() {
            return string.Join(SEPARATOR, _rate, _shortDescription, _description);
        }
    }
}

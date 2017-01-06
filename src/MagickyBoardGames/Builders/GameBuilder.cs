
using System;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders
{
    public class GameBuilder: IBuilder<Game, GameViewModel>
    {
        private int? _id;
        private string _name;
        private string _description;
        private int? _minPlayers;
        private int? _maxPlayers;

        public GameViewModel ToViewModel() {
            return new GameViewModel {
                Id = _id,
                Name = _name,
                Description = _description,
                MinPlayers = _minPlayers,
                MaxPlayers = _maxPlayers,
                PlayerRange = _minPlayers.HasValue && _maxPlayers.HasValue ? $"{_minPlayers.Value} - {_maxPlayers.Value}" : string.Empty
            };
        }

        public Game ToEntity() {
            if (!_id.HasValue || !_minPlayers.HasValue || !_maxPlayers.HasValue)
                throw new ArgumentException();

            return new Game {
                Id = _id.Value,
                Name = _name,
                Description = _description,
                MinPlayers = _minPlayers.Value,
                MaxPlayers = _maxPlayers.Value,
            };
        }

        public GameViewModel Build(Game entity) {
            _id = entity.Id;
            _name = entity.Name;
            _description = entity.Description;
            _minPlayers = entity.MinPlayers;
            _maxPlayers = entity.MaxPlayers;
            return ToViewModel();
        }

        public Game Build(GameViewModel viewModel) {
            _id = viewModel.Id;
            _name = viewModel.Name;
            _description = viewModel.Description;
            _minPlayers = viewModel.MinPlayers;
            _maxPlayers = viewModel.MaxPlayers;
            return ToEntity();
        }
    }
}

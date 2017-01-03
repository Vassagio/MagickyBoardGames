
using System;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Builders
{
    public class GameBuilder
    {
        private int? _id;
        private string _name;
        private string _description;
        private int? _minPlayers;
        private int? _maxPlayers;

        private GameViewModel ToViewModel() {
            return new GameViewModel {
                Id = _id,
                Name = _name,
                Description = _description,
                MinPlayers = _minPlayers,
                MaxPlayers = _maxPlayers,
                PlayerRange = _minPlayers.HasValue && _maxPlayers.HasValue ? $"{_minPlayers.Value} - {_maxPlayers.Value}" : string.Empty
            };
        }

        private Game ToObject() {
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

        public GameViewModel Build(Game game) {
            _id = game.Id;
            _name = game.Name;
            _description = game.Description;
            _minPlayers = game.MinPlayers;
            _maxPlayers = game.MaxPlayers;
            return ToViewModel();
        }

        public Game Build(GameViewModel viewModel) {
            _id = viewModel.Id;
            _name = viewModel.Name;
            _description = viewModel.Description;
            _minPlayers = viewModel.MinPlayers;
            _maxPlayers = viewModel.MaxPlayers;
            return ToObject();
        }
    }
}

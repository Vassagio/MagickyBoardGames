
using System;
using System.Linq;
using MagickyBoardGames.Models;
using MagickyBoardGames.ViewModels;
using System.Xml.Linq;

namespace MagickyBoardGames.Builders {
    public class GameBuilder : IBuilder<Game, GameViewModel>, IXmlElementBuilder<GameViewModel> {
        private const int SHORT_DESCRIPTION_LENGTH = 100;
        private int? _id;
        private string _name;
        private string _shortDescription;
        private string _description;
        private int? _minPlayers;
        private int? _maxPlayers;
        private string _image;
        private string _thumbnail;
        private int _gameId;

        public GameViewModel ToViewModel() {
            return new GameViewModel {
                Id = _id,
                Name = _name,
                Description = _description,
                ShortDescription = _shortDescription,
                MinPlayers = _minPlayers,
                MaxPlayers = _maxPlayers,
                Image = _image,
                Thumbnail = _thumbnail,
                GameId = _gameId,
                PlayerRange = _minPlayers.HasValue && _maxPlayers.HasValue ? $"{_minPlayers.Value} - {_maxPlayers.Value}" : string.Empty
            };
        }

        public Game ToEntity() {
            var entity = new Game {
                Name = _name,
                Description = _description,
                Image = _image,
                Thumbnail = _thumbnail
            };
            if (_id.HasValue)
                entity.Id = _id.Value;
            if (_minPlayers.HasValue)
                entity.MinPlayers = _minPlayers.Value;
            if (_maxPlayers.HasValue)
                entity.MaxPlayers = _maxPlayers.Value;
            return entity;
        }

        public GameViewModel Build(Game entity) {
            _id = entity.Id;
            _name = entity.Name;
            _description = entity.Description;
            _shortDescription = BuildShortDescription(_description);
            _minPlayers = entity.MinPlayers;
            _maxPlayers = entity.MaxPlayers;
            _image = entity.Image;
            _thumbnail = entity.Thumbnail;
            return ToViewModel();
        }

        public GameViewModel Build(XElement element) {
            _name = GetNameFromElement(element);
            _description = RemoveHtmlFromDescription(element.Element("description").Value);
            _shortDescription = BuildShortDescription(_description);
            _maxPlayers = int.Parse(element.Element("maxplayers").Attribute("value").Value);
            _minPlayers = int.Parse(element.Element("minplayers").Attribute("value").Value);
            _image = element.Element("image") != null ? element.Element("image").Value : string.Empty;
            _thumbnail = element.Element("thumbnail") != null ? element.Element("thumbnail").Value : string.Empty;
            _gameId = int.Parse(element.Attribute("id").Value);
            return ToViewModel();
        }

        private string GetNameFromElement(XElement element) {
            return element.Elements("name")
                .Where(e => e.Attribute("type").Value == "primary")
                .Select(e => e.Attribute("value").Value)
                .SingleOrDefault();
        }

        public Game Build(GameViewModel viewModel) {
            _id = viewModel.Id;
            _name = viewModel.Name;
            _description = viewModel.Description;
            _minPlayers = viewModel.MinPlayers;
            _maxPlayers = viewModel.MaxPlayers;
            _image = viewModel.Image;
            _thumbnail = viewModel.Thumbnail;
            return ToEntity();
        }

        private string RemoveHtmlFromDescription(string description) {
            return description.Replace("&#10;", string.Empty).
                               Replace("&mdash;", "-").
                               Replace("&ndash;", "-").
                               Replace("&quot;", "'");
        }

        private string BuildShortDescription(string description, int startIndex = 0) {
            if (string.IsNullOrEmpty(description)) return string.Empty;
            var position = description.IndexOf('.', startIndex);
            if (position == -1 && description.Length > SHORT_DESCRIPTION_LENGTH)
                return description.Substring(0, SHORT_DESCRIPTION_LENGTH);
            if (position == -1) return description;
            var result = description.Substring(0, position + 1);
            if (result.Length <= SHORT_DESCRIPTION_LENGTH && !result.Equals(description))
                return BuildShortDescription(description, position + 1);
            return result;
        }
    }
}

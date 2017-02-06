using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MagickyBoardGames.Builders;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Services.BoardGameGeek {
    public class BoardGameGeekService : IGameInfoService {
        private readonly IXmlElementBuilder<GameViewModel> _xmlElementBuilder;
        public const string BASE_URL = "http://www.boardgamegeek.com/xmlapi2";
        private const string ITEM = "item";
        private const string VALUE_ATTRIBUTE = "value";

        public BoardGameGeekService(IXmlElementBuilder<GameViewModel> xmlElementBuilder) {
            _xmlElementBuilder = xmlElementBuilder;
        }

        public async Task<IEnumerable<SearchResult>> Search(string query) {
            var searchUrl = $"/search?query={query}&type=boardgame";
            try {
                var teamDataUri = new Uri($"{BASE_URL}{searchUrl}");

                var xDoc = await ReadData(teamDataUri);

                var gameCollection = from boardGame in xDoc.Descendants(ITEM)
                                     select new SearchResult {
                                         Name = GetStringValue(boardGame.Element("name"), VALUE_ATTRIBUTE),
                                         GameId = GetIntValue(boardGame, "id")
                                     };
                return gameCollection;

            }
            catch (Exception ex) {
                return new List<SearchResult>();
            }
        }

        public async Task<GameViewModel> LoadGame(int gameId) {            
            var loadUrl = $"/thing?id={gameId}&stats=1";
            try {

                var teamDataUri = new Uri($"{BASE_URL}{loadUrl}");
                var xDoc = await ReadData(teamDataUri);

                var games = xDoc.Descendants("items")
                    .SelectMany(i => i.Descendants("item"))
                    .Select(i => _xmlElementBuilder.Build(i))
                    .ToList();

                return games.FirstOrDefault();
            }
            catch (Exception ex) {
                return new GameViewModel();
            }
        }

        public async Task<IEnumerable<GameViewModel>> LoadGames(params int[] gameIds) {
            var joinedIds = string.Join(",", gameIds);
            var loadUrl = $"/thing?id={joinedIds}&stats=1";
            try {

                var teamDataUri = new Uri($"{BASE_URL}{loadUrl}");
                var xDoc = await ReadData(teamDataUri);

                var games = xDoc.Descendants("items")
                    .SelectMany(i => i.Descendants("item"))
                    .Select(i => _xmlElementBuilder.Build(i))
                    .ToList();
      
                return games;
            }
            catch (Exception ex) {
                return new List<GameViewModel>();
            }
        }

        private static async Task<XDocument> ReadData(Uri requestUrl) {
            using (var client = new HttpClient()) {
                var responseBytes = await client.GetByteArrayAsync(requestUrl);
                var xmlResponse = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);

                return XDocument.Parse(xmlResponse);
            }
        }

        private static string GetStringValue(XElement element, string attribute = null, string defaultValue = "") {
            if (element == null)
                return defaultValue;

            if (attribute == null)
                return element.Value;

            var xatt = element.Attribute(attribute);
            return xatt == null ? defaultValue : xatt.Value;
        }
        private static int GetIntValue(XElement element, string attribute = null, int defaultValue = -1) {
            var val = GetStringValue(element, attribute, null);
            if (val == null)
                return defaultValue;

            int retVal;
            if (!int.TryParse(val, out retVal))
                retVal = defaultValue;
            return retVal;
        }
    }
}

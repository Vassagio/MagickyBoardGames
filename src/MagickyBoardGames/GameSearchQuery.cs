using System;
using System.Collections.Generic;
using System.Linq;
using MagickyBoardGames.Models;

namespace MagickyBoardGames {
    //TODO: Needs Testing
    public class GameSearchQuery : IGameSearchQuery {
        private readonly IQueryable<Game> _query;

        public GameSearchQuery(IQueryable<Game> query) {
            _query = query;
        }

        public IGameSearchQuery FilterByName(string name) {
            return string.IsNullOrEmpty(name) ? this : new GameSearchQuery(_query.Where(g => g.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)));
        }

        public IGameSearchQuery FilterByDescription(string description) {
            return string.IsNullOrEmpty(description) ? this : new GameSearchQuery(_query.Where(g => g.Description.Contains(description, StringComparison.CurrentCultureIgnoreCase)));
        }

        public IGameSearchQuery FilterByNumberOfPlayers(int? numberOfPlayers) {
            return numberOfPlayers.HasValue ? new GameSearchQuery(_query.Where(g => g.MinPlayers <= numberOfPlayers.Value && g.MaxPlayers >= numberOfPlayers)) : this;
        }

        public IGameSearchQuery FilterByAverageRating(int? averageRating) {
            var result = _query.SelectMany(g => g.GamePlayerRatings)
                               .GroupBy(gpr => new { Id = gpr.GameId })
                               .Select(g => new {
                                   AverageRating = g.Average(p => p.Rating.Rate),
                                   g.Key.Id
                               }).Where(r => r.AverageRating >= averageRating && r.AverageRating < averageRating + 1)
                               .Select(r => r.Id);
            return averageRating.HasValue ? new GameSearchQuery(_query.Where(i => result.Contains(i.Id))) : this;
        }

        public IEnumerable<Game> Execute() {
            return _query;
        }

        public int Count() {
            return _query.Count();
        }

        public IGameSearchQuery Limit(int count) {
            return count <= 0 ? this : new GameSearchQuery(_query.Take(count));
        }

        public IGameSearchQuery Offset(int count) {
            return new GameSearchQuery(_query.Skip(count));
        }
    }
}
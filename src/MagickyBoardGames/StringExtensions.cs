using System;

namespace MagickyBoardGames {
    //TODO: Needs Testing
    public static class StringExtensions {
        public static bool Contains(this string source, string value, StringComparison comparisonType) {
            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}
using System.Text.RegularExpressions;

namespace TCGPlayer_AutoCart
{
    /// <summary>
    /// Parses a decklist text file into a dictionary where the key is the card text
    /// (everything after the leading number) and the value is the numeric count.
    /// Lines like "Pokémon: 12", "Trainer: 17", "Energy: 3", and "Total Cards: 60"
    /// are ignored. Duplicate keys are aggregated (counts summed).
    /// </summary>
    static class DeckParser
    {
        public static List<(Card card, int quantity)> ParseDecklist(string filePath)
        {
            var result = new List<(Card card, int quantity)>();

            // Matches section header lines to exclude, e.g. "Pokémon: 12", "Trainer: 17", etc.
            var excludePattern = new Regex(@"^\s*(pok[eé]mon|trainer|energy|total\s+cards)\s*:\s*\d+\s*$",
                                           RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // Matches lines that start with a number followed by the card text.
            // Optionally captures a set code (2-4 uppercase letters) and a trailing card number.
            // Examples matched:
            //   "2 Cynthia's Gible DRI 102"
            //   "1 Cynthia's Gabite DRI 103 PH"
            //   "3 Arven PAF 235"
            var linePattern = new Regex(@"^\s*(\d+)\s+(.+?)(?:\s+([A-Z]{2,5})\s+(\d+))?\s*$",
                                        RegexOptions.CultureInvariant);

            foreach (var rawLine in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                var line = rawLine.Trim();

                if (excludePattern.IsMatch(line))
                {
                    continue;
                }

                var m = linePattern.Match(line);
                if (!m.Success)
                {
                    // Skip lines that don't match the expected "N text..." format.
                    continue;
                }

                if (!int.TryParse(m.Groups[1].Value, out var count))
                {
                    continue;
                }

                Card card = new Card();
                card.Name = m.Groups[2].Value.Trim();
                // If set code and card number were captured, assign them.
                if (m.Groups[3].Success && !string.IsNullOrWhiteSpace(m.Groups[3].Value))
                {
                    card.SetCode = m.Groups[3].Value.Trim();
                }

                if (m.Groups[4].Success && int.TryParse(m.Groups[4].Value, out var cardNum))
                {
                    card.CardNum = cardNum;
                }

                result.Add((card, count));
            }

            return result;
        }
    }
}

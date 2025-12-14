using System;
using System.Collections.Generic;
using System.Text;
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
        public static Dictionary<string, int> ParseDecklist(string filePath)
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Matches section header lines to exclude, e.g. "Pokémon: 12", "Trainer: 17", etc.
            var excludePattern = new Regex(@"^\s*(pok[eé]mon|trainer|energy|total\s+cards)\s*:\s*\d+\s*$",
                                           RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // Matches lines that start with a number followed by the card text.
            var linePattern = new Regex(@"^\s*(\d+)\s+(.+?)\s*$", RegexOptions.CultureInvariant);

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

                var key = m.Groups[2].Value.Trim();

                //maybe don't need this aggregation? we kinda know that the decklist won't have duplicates
                //if (result.ContainsKey(key))
                //{
                //    result[key] += count;
                //}
                //else
                //{
                result[key] = count;
                //}
            }

            return result;
        }
    }
}

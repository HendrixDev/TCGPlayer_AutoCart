using System;
using System.Collections.Generic;
using System.Text;

namespace TCGPlayer_AutoCart
{
    public static class SetParser
    {
        public static Dictionary<string, string> ParseSetList(string filePath)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var rawLine in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                var line = rawLine.Trim();

                var parts = line.Split('(');

                if (parts.Length != 2)
                {
                    continue;
                }

                var setName = parts[0].Trim();
                var setCode = parts[1].Trim(')');
                result[setCode] = setName;
            }
            return result;
        }
    }
}

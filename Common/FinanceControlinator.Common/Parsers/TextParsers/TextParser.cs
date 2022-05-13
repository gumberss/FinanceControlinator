using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceControlinator.Common.Parsers.TextParsers
{
    /// <summary>
    /// It should be a microservice?
    /// in the future, probably business rules will be inserted here,
    /// like "Should Capitalize when replace", something like this
    /// </summary>
    public class TextParser : ITextParser
    {
        public String Parse(String message, params (String key, String value)[] parsers)
        {
            return Parse(message, parsers.AsEnumerable());
        }

        private String Parse(String message, IEnumerable<(String key, String value)> parsers)
        {
            if (!parsers.Any()) return message;

            var (key, value) = parsers.First();

            return Parse(message.Replace($"[[{key}]]", value), parsers);
        }
    }
}

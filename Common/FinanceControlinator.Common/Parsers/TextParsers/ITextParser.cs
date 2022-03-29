using System;
using System.Collections.Generic;

namespace FinanceControlinator.Common.Parsers.TextParsers
{
    public interface ITextParser
    {
        String Parse(String message, IEnumerable<(String key, String value)> parsers);
    }
}

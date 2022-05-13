﻿using System;
using System.Collections.Generic;

namespace FinanceControlinator.Common.Parsers.TextParsers
{
    public interface ITextParser
    {
        String Parse(String message, params (String key, String value)[] parsers);
    }
}

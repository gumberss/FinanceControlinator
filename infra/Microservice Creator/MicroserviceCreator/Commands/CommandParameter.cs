using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Commands
{
    public class CommandParameter
    {
        private readonly string _parameterText;

        public CommandParameter(String parameter)
        {
            _parameterText = parameter;

            var param = parameter.Replace("--", String.Empty);

            if (parameter.Contains("="))
            {
                var parts = param.Split("=");

                Key = parts[0];
                Value = parts[1];
            }
            else
            {
                Key = param;
            }
        }

        public String Key { get; private set; }

        public String Value { get; private set; }

        public bool HasValue() => Value != null;
        
        public override string ToString() => _parameterText;

        public static implicit operator CommandParameter(String parameter)
            => new CommandParameter(parameter);

        public static implicit operator String(CommandParameter parameter)
            => parameter.ToString();
    }
}

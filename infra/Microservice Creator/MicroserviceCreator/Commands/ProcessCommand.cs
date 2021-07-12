using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Commands
{
    public class ProcessCommand
    {
        public ProcessCommand(String fullCommandText)
        {
            if (!String.IsNullOrEmpty(fullCommandText))
            {
                FullCommand = fullCommandText;

                var commandToExecute = fullCommandText
                    .Split('-')
                    .First();

                Command = commandToExecute
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                Parameters = fullCommandText
                    .Replace(commandToExecute, String.Empty)
                    .Split('-', StringSplitOptions.RemoveEmptyEntries)
                    .Select(parameterText => new CommandParameter(parameterText.Trim()))
                    .ToList();
            }
        }

        public CommandParameter FindParameter(String key)
            => Parameters.Find(x => x.Key == key);
        

        public List<String> Command { get; private set; }

        public List<CommandParameter> Parameters { get; private set; }

        public String FullCommand { get; private set; }

        public static implicit operator ProcessCommand(String command)
        {
            return new ProcessCommand(command);
        }

        public static implicit operator ProcessCommand(String[] command)
        {
            return new ProcessCommand(String.Join(' ', command));
        }

        public static implicit operator String(ProcessCommand command)
        {
            return command.FullCommand;
        }

        public bool IsCommandEquals(ProcessCommand command)
           => this.Command.SequenceEqual(command.Command);

        public bool IsFullCommandEquals(ProcessCommand command)
            => this.FullCommand.SequenceEqual(command.FullCommand);

        public String Details()
        {
            return $@"Complete Command: {FullCommand}. 
                    Converted Command:
                    Commands: {String.Join(' ', Command)}
                    Parameters: {String.Join(' ', Parameters)}";
        }
    }
}

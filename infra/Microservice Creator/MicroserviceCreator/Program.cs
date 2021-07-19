using Cli.Actions;
using Cli.Actions.Builds;
using Cli.Chains;
using Cli.Commands;
using Cli.Notifications;
using Cli.Validations.Commands;
using System.Collections.Generic;

namespace Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleNotification = new ConsoleNotification();

            var classesFinder = new ClassesFinder()
                .Subscribe(consoleNotification);

            //var commandValidations = classesFinder.GetClassesAssignableFrom<ICommandValidation>(); // automagico

            var commandValidations = new List<ICommandValidation>
            {
                //It must be the first one
                new MinimumParametersQuantity()
            };

            var isValid = new ChainExecuter()
                .Execute(args, commandValidations, consoleNotification);

            if (!isValid.Result) return;

            //var commandClasses = classesFinder.GetClassesAssignableFrom<IAction>(); // automagico

            var commandClasses = new List<IAction>
            {
                new BuildMicroservice(),

                //It must be the last one
                new NoActionFound()
            };

            new ChainExecuter()
                .Execute(args, commandClasses, consoleNotification);

            //if (allProcessWereIgnored) // if you select automagico, uncomment this
            //{
            //    consoleNotification.Notify($"No actions Found. {((ProcessCommand)args).Details()}");
            //}
        }
    }
}

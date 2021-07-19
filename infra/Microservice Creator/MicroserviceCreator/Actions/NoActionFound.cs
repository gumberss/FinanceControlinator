using Cli.Chains;
using Cli.Commands;
using System.Threading.Tasks;

namespace Cli.Actions
{
    public class NoActionFound : Chain<ProcessCommand>, IAction
    {
        public override Task<bool> ChainWrapper(ProcessCommand command)
        {
            Notify($"No actions Found. {command.Details()}");

            Notify($"Do you want help of one specific command? type the command with flag '--help'. Example: 'ask for --help'");

            return Task.FromResult(false);
        }
    }
}

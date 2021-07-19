
using Cli.Chains;
using Cli.Commands;
using System.Threading.Tasks;

namespace Cli.Validations.Commands
{
    public class MinimumParametersQuantity : Chain<ProcessCommand>, ICommandValidation
    {
        private readonly int MINIMUM_PARAMETER_QUANTITY = 1;

        public async override Task<bool> ChainWrapper(ProcessCommand command)
        {
            if(command.Command is null || command.Command.Count < MINIMUM_PARAMETER_QUANTITY)
            {
                Notify($"The command sent have less words than acepted. Quantity: {command.Command?.Count ?? 0}. Minimum: {MINIMUM_PARAMETER_QUANTITY}.");
                return false;
            }

            return true;
        }
    }
}

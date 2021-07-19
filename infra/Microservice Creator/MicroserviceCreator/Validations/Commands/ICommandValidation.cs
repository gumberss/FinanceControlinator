using Cli.Chains;
using Cli.Commands;

namespace Cli.Validations.Commands
{
    public interface ICommandValidation : IChain<ProcessCommand>
    {
    }
}

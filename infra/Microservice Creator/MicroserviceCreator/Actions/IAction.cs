using Cli.Chains;
using Cli.Commands;

namespace Cli.Actions
{
    public interface IAction : IChain<ProcessCommand>
    {
         //ProcessCommand Command { get; }
    }
}

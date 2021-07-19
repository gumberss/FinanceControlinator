using Cli.Chains;
using Cli.Commands;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cli.Actions
{
    public abstract class ActionChain : Chain<ProcessCommand>, IAction
    {
        public abstract ProcessCommand Command { get; }

        public override async Task<bool> ChainWrapper(ProcessCommand command)
        {
            var processIgnored = true;

            if (this.Command.IsCommandEquals(command))
            {
                Stopwatch s = new Stopwatch();
                s.Start();

                try
                {
                    if (command.FindParameter("help") != null)
                    {
                        await Help(command);
                    }
                    else
                    {
                        //validade
                        await Execute(command);
                    }
                }
                finally
                {
                    s.Stop();

                    Notify($"Time to execute: {s.ElapsedMilliseconds} milliseconds");
                }

                processIgnored = false;
            }

            return processIgnored;
        }

        protected abstract Task Execute(ProcessCommand command);

        protected abstract Task Help(ProcessCommand command);
    }
}

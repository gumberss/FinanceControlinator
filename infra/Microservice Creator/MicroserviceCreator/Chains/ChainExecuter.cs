using Cli.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cli.Chains
{
    public class ChainExecuter
    {
        public async Task<bool> Execute<T>(T command, IEnumerable<IChain<T>> chain, params INotification[] notifications)
        {
            if (!chain.Any()) return true;

            var first = chain.First();

            CreateChain(chain, notifications, chain.First(), chain.Skip(1).FirstOrDefault());

            return await first.ExecuteChain(command);
        }

        private void CreateChain<T>(
            IEnumerable<IChain<T>> chain
            , IEnumerable<INotification> notifications
            , IChain<T> current
            , IChain<T> next)
        {
            current.Subscribe(notifications);

            if (!chain.Any()) return;

            current.SetNext(next);

            if (next == null) return;

            CreateChain(chain.Skip(1), notifications, chain.First(), chain.Skip(1).FirstOrDefault());
        }

    }
}

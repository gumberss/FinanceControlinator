using Cli.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Chains
{
    public interface IChain<T>
    {
        IChain<T> Subscribe(INotification notification);
        IChain<T> Subscribe(IEnumerable<INotification> notifications);

        IChain<T> Unsubscribe(INotification notification);
        IChain<T> Unsubscribe(IEnumerable<INotification> notifications);

        public IChain<T> SetNext(IChain<T> next);

        Task<bool> ExecuteChain(T data);
    }
}

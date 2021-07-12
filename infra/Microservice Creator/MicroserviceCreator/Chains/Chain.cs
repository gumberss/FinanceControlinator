
using Cli.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cli.Chains
{
    public abstract class Chain<T> : IChain<T>
    {
        private IChain<T> _next;

        private readonly List<INotification> _notifications;

        protected Chain()
        {
            _notifications = new List<INotification>();
        }

        public IChain<T> Subscribe(IEnumerable<INotification> notifications)
        {
            foreach (var notification in notifications) Subscribe(notification);

            return this;
        }

        public IChain<T> Subscribe(INotification notification)
        {
            if (!_notifications.Contains(notification))
                _notifications.Add(notification);

            return this;
        }

        public IChain<T> Unsubscribe(IEnumerable<INotification> notifications)
        {
            foreach (var notification in notifications) Unsubscribe(notification);

            return this;
        }

        public IChain<T> Unsubscribe(INotification notification)
        {
            if (_notifications.Contains(notification))
                _notifications.Remove(notification);

            return this;
        }

        public IChain<T> SetNext(IChain<T> next)
        {
            _next = next;

            return next;
        }

        protected void Notify(string message)
        {
            _notifications.ForEach(x => x.Notify(message));
        }

        public async Task<bool> ExecuteChain(T data)
        {
            try
            {
                return await ChainWrapper(data) && (_next == null || await _next?.ExecuteChain(data));
            }
            catch (Exception ex)
            {
                Notify($"An error occured: {ex}");
                return false;
            }
        }

        public abstract Task<bool> ChainWrapper(T data);
    }
}

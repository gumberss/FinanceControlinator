﻿using FinanceControlinator.Common.Messaging;
using MassTransit;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Handler.Configurations
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public MassTransitMessageBus(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            return _bus.Publish(message, cancellationToken);
        }
    }
}

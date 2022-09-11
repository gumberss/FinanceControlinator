using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.IntegrationTests.TestFactories
{
    public class FakeConsumer<T> : IConsumer, IConsumer<T> where T : class
    {
        private readonly Action<T> _callback;

        public FakeConsumer() => _callback = (_) => { };
        public FakeConsumer(Action<T> callback) => _callback = callback;

        public Task Consume(ConsumeContext<T> context)
        {
            _callback(context.Message);

            return Task.CompletedTask;
        }
    }

    public class MassTransitFacker : IFakeConfig<InMemoryTestHarness>
    {
        private List<IConsumer> _consumers;
        InMemoryTestHarness? _harness;

        public MassTransitFacker()
        {
            _consumers = new List<IConsumer>();
        }

        public MassTransitFacker WithConsumers(params IConsumer[] consumers)
        {
            _consumers = consumers.ToList();
            return this;
        }

        public async Task Configure(IServiceCollection services)
        {
            services
            .AddMassTransitInMemoryTestHarness(cfg =>
            {
                _consumers.ForEach(x => cfg.AddConsumer(x.GetType()));
            });

            var provider = services.BuildServiceProvider();
            _harness = provider.GetRequiredService<InMemoryTestHarness>();
            _harness.TestTimeout = TimeSpan.FromMinutes(1);
            //https://github.com/MassTransit/MassTransit/issues/2544#issuecomment-884858167
            _harness.TestInactivityTimeout = TimeSpan.FromSeconds(1);
            await _harness.Start();

            services.AddTransient((_) => _harness.Bus);
        }

        public InMemoryTestHarness Get(IServiceProvider provider) => _harness!;
    }
}

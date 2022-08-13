using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MassTransit.Testing;
using MassTransit;


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

    public class MassTransitFacker
    {
        public async Task<InMemoryTestHarness> ConfigureMassTransit(IServiceCollection services,  params IConsumer[] consumers)
        {
            services
             .AddMassTransitInMemoryTestHarness(cfg =>
             {
                 foreach (var consumer in consumers)
                     cfg.AddConsumer(consumer.GetType());
             });

            var provider = services.BuildServiceProvider();
            var harness = provider.GetRequiredService<InMemoryTestHarness>();
            await harness.Start();

            services.AddScoped<IBus>((_) => harness.Bus);

            return harness;
        }
    }
}

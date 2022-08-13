using Expenses.IntegrationTests.TestFactories;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.IntegrationTests
{




    public class PongMessage
    {

    }
    public class TEstinho
    {

        [Fact]
        public async void a()
        {
            try
            {
                var serviceCollection = new ServiceCollection();

                var harness = await new MassTransitFacker()
                    .ConfigureMassTransit(serviceCollection, new FakeConsumer<InvoicePaidEvent>());
                var provider = serviceCollection.BuildServiceProvider(true);
                var bus = provider.GetRequiredService<IBus>();
                await bus.Publish(new InvoicePaidEvent());

                var aa = await harness.Consumed.Any(x => true);
            }
            catch (Exception ex)
            {

            }


        }

        [Fact]
        public async void B()
        {
            try
            {

                var w = new WebApplication();
                var client = w.Mock2(Guid.NewGuid());

                var b = await client.GetAsync("api/expenses/a");
            }
            catch (Exception ex)
            {

            }


        }
    }
}

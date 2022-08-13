using AutoFixture;
using Expenses.DTO.Expenses;
using Expenses.IntegrationTests.TestFactories;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.IntegrationTests
{




    public class PongMessage
    {

    }
    public class SimpleTest
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

                var w = new WebApplicationMockBuilder();
                var (client, harness) = w.WithConsumers(new FakeConsumer<InvoicePaidEvent>())
                    .WithAuthorizedUser(Guid.NewGuid())
                    .Build();
                Fixture fixture = new Fixture();

                var a = fixture.Create<ExpenseDTO>();

                var b = await client.PostAsync("api/expenses", JsonContent.Create(a));
            }
            catch (Exception ex)
            {

            }


        }
    }
}

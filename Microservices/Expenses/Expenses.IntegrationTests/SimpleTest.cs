using AutoFixture;
using Expenses.DTO.Expenses;
using Expenses.IntegrationTests.TestFactories;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using MassTransit.Testing;
using System;
using System.Linq;
using System.Net.Http.Json;
using Xunit;

namespace Expenses.IntegrationTests
{
    public class SimpleTest
    {
        private WebApplicationMockBuilder _apiBuilder;
        private MassTransitFacker _massTransitFacker;
        private Guid _userId;

        public SimpleTest()
        {
            _apiBuilder = new WebApplicationMockBuilder();
            _massTransitFacker = new MassTransitFacker();
            _userId = Guid.NewGuid();

            _apiBuilder
                .AddServiceConfiguration(_massTransitFacker)
                .WithAuthorizedUser(_userId);
        }


        //[Fact]
        //public async void OnlyMessageSimpleIntegrationTest()
        //{
        //    try
        //    {
        //        var serviceCollection = new ServiceCollection();

        //        var harness = await new MassTransitFacker()
        //            .ConfigureMassTransit(serviceCollection, new FakeConsumer<InvoicePaidEvent>());
        //        var provider = serviceCollection.BuildServiceProvider(true);
        //        var bus = provider.GetRequiredService<IBus>();
        //        await bus.Publish(new InvoicePaidEvent());

        //        var aa = await harness.Consumed.Any(x => true);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[Fact]
        //public async void FirstSimpleIntegrationTest()
        //{
        //    try
        //    {
        //        var w = new WebApplicationMockBuilder();
        //        var (client, harness) = w.WithConsumers(new FakeConsumer<GenerateInvoicesEvent>())
        //            .WithAuthorizedUser(Guid.NewGuid())
        //            .Build();
        //        Fixture fixture = new Fixture();

        //        var expense = fixture.Create<ExpenseDTO>();
        //        expense.TotalCost = expense.Items.Sum(x => x.Cost * x.Amount);

        //        var apiResult = await client.PostAsync("api/expenses", JsonContent.Create(expense));
        //        var messagesResult =await  harness.Consumed.Any();
        //        var messagesResult2 = await harness.Consumed.Any<GenerateInvoicesEvent>();
        //        var messagesResult3 = harness.Consumed.Select<GenerateInvoicesEvent>();
        //        var theEvent = messagesResult3.ToList().First().Context.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[Fact]
        //public async void SecondSimpleIntegrationTest()
        //{
        //    try
        //    {
        //        _massTransitFacker.WithConsumers(new FakeConsumer<GenerateInvoicesEvent>());
        //        var client = _apiBuilder.Build();

        //        Fixture fixture = new Fixture();

        //        var expense = fixture.Create<ExpenseDTO>();
        //        expense.TotalCost = expense.Items.Sum(x => x.Cost * x.Amount);

        //        var apiResult = await client.PostAsync("api/expenses", JsonContent.Create(expense));
        //        var messagesResult = await _harness!.Consumed.Any();
        //        var messagesResult2 = await _harness.Consumed.Any<GenerateInvoicesEvent>();
        //        var messagesResult3 = _harness.Consumed.Select<GenerateInvoicesEvent>();
        //        var theEvent = messagesResult3.ToList().First().Context.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        //[Fact]
        //public async void SecondSimpleIntegrationTest2()
        //{
        //    try
        //    {
        //        _massTransitFacker.WithConsumers(new FakeConsumer<GenerateInvoicesEvent>());
        //        var client = _apiBuilder.Build();

        //        Fixture fixture = new Fixture();

        //        var expense = fixture.Create<ExpenseDTO>();
        //        expense.TotalCost = expense.Items.Sum(x => x.Cost * x.Amount);

        //        var apiResult = await client.PostAsync("api/expenses", JsonContent.Create(expense));
        //        var messagesResult = await _harness!.Consumed.Any();
        //        var messagesResult2 = await _harness.Consumed.Any<GenerateInvoicesEvent>();
        //        var messagesResult3 = _harness.Consumed.Select<GenerateInvoicesEvent>();
        //        var theEvent = messagesResult3.ToList().First().Context.Message;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

    }
}

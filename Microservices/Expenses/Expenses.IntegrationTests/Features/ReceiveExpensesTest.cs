using AutoFixture;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Data.Repositories;
using Expenses.DTO.Expenses;
using Expenses.IntegrationTests.TestFactories;
using FinanceControlinator.Events.Invoices;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Expenses.IntegrationTests.Features
{
    public class ReceiveExpensesTest : IDisposable
    {
        private readonly WebApplicationMockBuilder _apiBuilder;
        private readonly Guid _userId;
        private readonly Fixture _fixture;
        private readonly HttpClient _client;
        private readonly InMemoryTestHarness? _harness;
        private readonly IExpenseRepository _expenseRepository;

        public ReceiveExpensesTest()
        {
            (_userId, _fixture) = (Guid.NewGuid(), new Fixture());

            _apiBuilder = new WebApplicationMockBuilder()
                .AddServiceConfiguration(new MassTransitFacker())
                .AddServiceConfiguration(new EntityFrameworkFacker())
                .WithAuthorizedUser(_userId);

            _client = _apiBuilder.Build();

            _harness = _apiBuilder.GetFake<InMemoryTestHarness>();
            _expenseRepository = _apiBuilder.GetService<IExpenseRepository>()!;
        }

        [Fact]
        public async void Should_insert_a_new_expense()
        {
            var expense = _fixture.Create<ExpenseDTO>();
            expense.TotalCost = expense.Items.Sum(x => x.Cost * x.Amount);

            var apiResult = await _client.PostAsync("api/expenses", JsonContent.Create(expense));

            _harness!.Published.Count().Should().Be(1);
            _harness!.Published.Select<GenerateInvoicesEvent>().Should().HaveCount(1);
            var publishedEvent = _harness!.Published.Select<GenerateInvoicesEvent>().First();
            var invoiceExpense = publishedEvent.Context.Message.InvoiceExpense;

            var insertedExpense = (await _expenseRepository.GetAllAsync()).Value.SingleOrDefault();

            insertedExpense.Should().NotBeNull();

            invoiceExpense.Title.Should().Be(expense.Title);
            invoiceExpense.InstallmentsCount.Should().Be(expense.InstallmentsCount);
            invoiceExpense.Location.Should().Be(expense.Location);
            invoiceExpense.TotalCost.Should().Be(expense.TotalCost);
            invoiceExpense.Type.Should().Be((int)expense.Type);
            //invoiceExpense.PurchaseDay.Should().Be(expense.PurchaseDate);
            //invoiceExpense.Date.Should().Be(expense.PurchaseDate);
        }

        public void Dispose()
        {
            _apiBuilder.Dispose();
        }
    }
}

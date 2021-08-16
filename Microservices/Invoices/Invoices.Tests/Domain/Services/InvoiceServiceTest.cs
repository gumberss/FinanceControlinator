using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Tests.Domain.Services
{
    [TestClass]
    public class InvoiceServiceTest
    {
        private static InvoiceService _invoiceService;
        private static DateTime _purchaseDay;
        private Expense _expense;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _invoiceService = new InvoiceService();
            _purchaseDay = new DateTime(2021, 08, 05);
        }

        [TestInitialize]
        public void Init()
        {
            _expense = new Expense()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Expense",
                InstallmentsCount = 2,
                TotalCost = 100,
                Location = "Location",
                PurchaseDay = _purchaseDay,
                Type = InvoiceItemType.Investment
            };
        }

        [TestMethod]
        public void Should_create_a_new_invoice_when_a_new_expense_was_informed()
        {
            _expense.InstallmentsCount = 1;
            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(1);
            invoices.First().TotalCost.Should().Be(100);
            invoices.First().Items.Should().HaveCount(1);

            var invoiceItemFromExpense = invoices.First().Items.First();

            invoiceItemFromExpense.InstallmentCost.Should().Be(_expense.TotalCost, because: "The expense has only one installment");
            invoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            invoiceItemFromExpense.Title.Should().Be(_expense.Title);
        }

        [TestMethod]
        public void Should_create_two_new_invoices_when_the_informed_expense_was_buy_in_two_installments()
        {
            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);
            invoices.First().TotalCost.Should().Be(50);
            invoices.First().Items.Should().HaveCount(1);

            invoices.Last().TotalCost.Should().Be(50);
            invoices.Last().Items.Should().HaveCount(1);

            var firstInvoiceItemFromExpense = invoices.First().Items.First();

            firstInvoiceItemFromExpense.InstallmentCost.Should().Be(50);
            firstInvoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            firstInvoiceItemFromExpense.Title.Should().Be(_expense.Title);
            firstInvoiceItemFromExpense.Location.Should().Be(_expense.Location);
            firstInvoiceItemFromExpense.PurchaseDay.Should().Be(_expense.PurchaseDay);
            firstInvoiceItemFromExpense.Type.Should().Be(_expense.Type);

            var secondInvoiceItemFromExpense = invoices.Last().Items.First();

            secondInvoiceItemFromExpense.InstallmentCost.Should().Be(50);
            secondInvoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            secondInvoiceItemFromExpense.Title.Should().Be(_expense.Title);
            secondInvoiceItemFromExpense.Location.Should().Be(_expense.Location);
            secondInvoiceItemFromExpense.PurchaseDay.Should().Be(_expense.PurchaseDay);
            secondInvoiceItemFromExpense.Type.Should().Be(_expense.Type);
        }

        [TestMethod]
        public void Should_add_the_expense_in_an_invoice_when_that_month_invoice_already_exists()
        {
            var purchaseDay = new DateTime(2021, 08, 05);

            _expense.PurchaseDay = purchaseDay;

            var existentInvoice = new Invoice(closeDate: purchaseDay)
            {
                Id = Guid.NewGuid().ToString(),
            };
            
            existentInvoice.AddNew(new InvoiceItem(1, 100));

            var existentInvoices = new List<Invoice>() { existentInvoice };

            var currentInvoiceDate = purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);

            var newInvoices = invoices.Except(existentInvoices);
            newInvoices.Should().HaveCount(1);
            var newInvoice = newInvoices.First();
            newInvoice.Items.Should().HaveCount(1);

            var newInvoiceItem = newInvoices.First().Items.First();
            newInvoiceItem.InstallmentCost.Should().Be(50);
            newInvoiceItem.ExpenseId.Should().Be(_expense.Id);
            newInvoiceItem.Title.Should().Be(_expense.Title);
            newInvoiceItem.Location.Should().Be(_expense.Location);
            newInvoiceItem.PurchaseDay.Should().Be(_expense.PurchaseDay);
            newInvoiceItem.Type.Should().Be(_expense.Type);

            existentInvoice.Items.Should().HaveCount(2, because: "First must be the existent item, the second one must be the new item from new expense");

            var existentInvoiceItem = existentInvoice.Items.Last();
            existentInvoiceItem.InstallmentCost.Should().Be(50);
            existentInvoiceItem.ExpenseId.Should().Be(_expense.Id);
            existentInvoiceItem.Title.Should().Be(_expense.Title);
            existentInvoiceItem.Location.Should().Be(_expense.Location);
            existentInvoiceItem.PurchaseDay.Should().Be(_expense.PurchaseDay);
            existentInvoiceItem.Type.Should().Be(_expense.Type);
        }

        [TestMethod]
        public void Should_create_a_new_invoice_when_the_month_invoice_already_exist_but_of_the_other_year()
        {
            var existentInvoiceDate = new DateTime(2004, 08, 05);

            var existentInvoice = new Invoice(closeDate: existentInvoiceDate)
            {
                Id = Guid.NewGuid().ToString()
            };
            existentInvoice.AddNew(new InvoiceItem(1, 100));

            var existentInvoices = new List<Invoice>() { existentInvoice };

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);

            invoices.Except(existentInvoices).Should().HaveCount(2);
        }

        [TestMethod]
        public void Should_put_the_rest_of_the_expense_cost_on_the_last_invoice()
        {
            _expense.InstallmentsCount = 3;
            _expense.TotalCost = 100; // 100/3 = 33.33 * 3 = 99.99 + .01 -> rest

            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(3);

            invoices.First().TotalCost.Should().Be(33.33m);
            invoices.First().Items.First().InstallmentCost.Should().Be(33.33m);
            
            invoices[1].TotalCost.Should().Be(33.33m);
            invoices[1].Items.First().InstallmentCost.Should().Be(33.33m);

            invoices.Last().TotalCost.Should().Be(33.34m, because: "Adding the rest of the expense cost");
            invoices.Last().Items.First().InstallmentCost.Should().Be(33.34m, because: "Adding the rest of the expense cost");
        }
    }
}

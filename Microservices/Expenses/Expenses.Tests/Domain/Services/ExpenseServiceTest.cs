using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using Expenses.Domain.Services;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Expenses.Tests.Domain.Services
{
    [TestClass]
    public class ExpenseServiceTest
    {
        private IExpenseService _service;

        [TestInitialize]
        public void Init()
        {
            _service = new ExpenseService();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseUpdate)]
        public void Should_return_valid_when_the_expense_cost_is_greater_than_its_cost_in_paid_invoice()
        {
            var expense = new Expense()
            {
                Id = Guid.NewGuid(),
                TotalCost = 20
            };

            var invoice1 = new Invoice();
            invoice1.Items.Add(new InvoiceItem
            {
                ExpenseId = expense.Id,
                InstallmentCost = 10
            });

            List<Invoice> paidInvoices = new List<Invoice>()
            {
                invoice1
            };

            var isValid = _service.TotalCostIsValid(expense, paidInvoices);

            isValid.Should().BeTrue();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseUpdate)]
        public void Should_return_valid_when_the_expense_cost_is_greater_than_its_cost_in_paid_invoices()
        {
            var expense = new Expense()
            {
                Id = Guid.NewGuid(),
                TotalCost = 20
            };

            var invoice1 = new Invoice();
            invoice1.Items.Add(new InvoiceItem
            {
                ExpenseId = expense.Id,
                InstallmentCost = 10
            });

            var invoice2 = new Invoice();
            invoice2.Items.Add(new InvoiceItem
            {
                ExpenseId = expense.Id,
                InstallmentCost = 5
            });

            List<Invoice> paidInvoices = new List<Invoice>()
            {
                invoice1,
                invoice2
            };

            var isValid = _service.TotalCostIsValid(expense, paidInvoices);

            isValid.Should().BeTrue();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseUpdate)]
        public void Should_return_invalid_when_the_expense_cost_is_less_than_its_cost_in_paid_invoice()
        {
            var expense = new Expense()
            {
                Id = Guid.NewGuid(),
                TotalCost = 20
            };

            var invoice1 = new Invoice();
            invoice1.Items.Add(new InvoiceItem
            {
                ExpenseId = expense.Id,
                InstallmentCost = 100
            });

            List<Invoice> paidInvoices = new List<Invoice>()
            {
                invoice1
            };

            var isValid = _service.TotalCostIsValid(expense, paidInvoices);

            isValid.Should().BeFalse();
        }
    }
}

using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Invoices.Tests.Domain.Models
{
    [TestClass]
    public class InvoiceTest
    {
        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.InvoicePayment)]
        [IntegrationTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.Payment)]
        public void Should_set_an_invoice_as_paid_when_was_informed_that_it_was_paid()
        {
            var invoice = new Invoice(DateTime.Now);

            var datePaid = DateTime.Now;

            invoice.WasPaidIn(datePaid);

            invoice.PaymentDate.Should().Be(datePaid);
            invoice.PaymentStatus.Should().Be(PaymentStatus.Paid);
            invoice.UpdatedDate.Should().BeOnOrAfter(datePaid);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [IntegrationTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Should_add_an_item_to_invoce()
        {
            var invoice = new Invoice(DateTime.Now);
            var item = new InvoiceItem(10, 100);
            
            invoice.AddNew(item);

            invoice.Items.Should().HaveCount(1);
            invoice.Items.First().Should().Be(item);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [IntegrationTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Should_change_invoice_total_cost_when_a_item_is_added()
        {
            var invoice = new Invoice(DateTime.Now);
            var itemCost = 100;

            var item = new InvoiceItem(10, itemCost);

            invoice.AddNew(item);

            invoice.TotalCost.Should().Be(itemCost);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [IntegrationTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Should_return_the_sum_of_item_costs_as_invoice_total_cost()
        {
            var invoice = new Invoice(DateTime.Now);

            var item1Cost = 50;
            var item2Cost = 100;
            var totalCost = item1Cost + item2Cost;

            var item1 = new InvoiceItem(10, item1Cost);
            var item2 = new InvoiceItem(10, item2Cost);

            invoice
                .AddNew(item1)
                .AddNew(item2);

            invoice.TotalCost.Should().Be(totalCost);
        }
    }
}

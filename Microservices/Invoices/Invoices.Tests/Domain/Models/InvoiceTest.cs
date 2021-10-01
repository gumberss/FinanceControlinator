﻿using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

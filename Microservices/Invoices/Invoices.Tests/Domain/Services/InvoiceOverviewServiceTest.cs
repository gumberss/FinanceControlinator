using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Models.Sync;
using Invoices.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Invoices.Tests.Domain.Services
{
    [TestClass]
    [JourneyCategory(TestUserJourneyEnum.Overview)]
    [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.Overview)]
    public class InvoiceOverviewServiceTest
    {
        private InvoiceOverviewService _invoiceOverviewService;
        private readonly ILocalization _localization;

        public InvoiceOverviewServiceTest()
        {
            _invoiceOverviewService = new InvoiceOverviewService();

            _localization = new Ptbr();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); // convert "99.99" to 99.99 correctly in any computer
        }

        [TestMethod]
        public void Should_return_the_invoice_text()
            => _invoiceOverviewService
                .InvoiceCloseDateText(new Invoice(new DateTime(2021, 09, 20)))
                .Should().Be("09/2021");

        [TestMethod]
        [DataRow("100", "0", "100")]
        [DataRow("50", "50", "50")]
        [DataRow("0", "100", "0")]
        [DataRow("1", "99", "1")]
        [DataRow("300", "200", "60")]
        [DataRow("99.99", "00.01", "99.99")]
        public void Should_return_the_future_purchase_percent_correctly(string totalFuturePurchase, string totalOthers, string expectedPercent)
            => _invoiceOverviewService.FuturePurchasePercent(new Invoice(DateTime.Now)
                .AddNew(new InvoiceItem(1, decimal.Parse(totalFuturePurchase)).WithType(InvoiceItemType.PiggyBank))
                .AddNew(new InvoiceItem(2, decimal.Parse(totalOthers)).WithType(InvoiceItemType.Bill)))
            .Should().Be(decimal.Parse(expectedPercent));

        [TestMethod]
        [DataRow("100", "0", "100")]
        [DataRow("50", "50", "50")]
        [DataRow("0", "100", "0")]
        [DataRow("1", "99", "1")]
        [DataRow("300", "200", "60")]
        [DataRow("99.99", "00.01", "99.99")]
        public void Should_return_the_investment_percent_correctly(string totalInvestment, string totalOthers, string expectedPercent)
        => _invoiceOverviewService.InvestmentPercent(new Invoice(DateTime.Now)
                .AddNew(new InvoiceItem(1, decimal.Parse(totalInvestment)).WithType(InvoiceItemType.Investment))
                .AddNew(new InvoiceItem(2, decimal.Parse(totalOthers)).WithType(InvoiceItemType.Bill)))
            .Should().Be(decimal.Parse(expectedPercent));

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_spent_in_bill_this_month_and_the_average_of_the_last_six_when_have_increase_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 10, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 150).WithType(InvoiceItemType.Bill));

            _invoiceOverviewService
                .BillAverageSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(50, because: "the average of the last 6 monthes was 100, and in this current month was 150");
        }

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_spent_in_bill_this_month_and_the_average_of_the_last_six_when_have_decrease_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 10, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 80).WithType(InvoiceItemType.Bill));

            _invoiceOverviewService
                .BillAverageSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(-20, because: "the average of the last 6 monthes was 10, and in this current month was 80");
        }

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_spent_in_bill_this_month_and_the_average_of_the_last_six_when_have_no_change_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 10, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill));

            _invoiceOverviewService
                .BillAverageSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(0);
        }

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_invoice_cost_this_month_and_the_average_of_the_last_six_when_have_have_increase_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20))
                    .AddNew(new InvoiceItem(1, 20).WithType(InvoiceItemType.Bill))
                    .AddNew(new InvoiceItem(1, 80).WithType(InvoiceItemType.Investment)),
                new Invoice(new DateTime(2021, 10, 20))
                    .AddNew(new InvoiceItem(1, 10).WithType(InvoiceItemType.PiggyBank))
                    .AddNew(new InvoiceItem(1, 90).WithType(InvoiceItemType.Health)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Leisure)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Market)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Other)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 150).WithType(InvoiceItemType.Bill));

            _invoiceOverviewService
                .InvoiceSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(50);
        }

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_invoice_cost_this_month_and_the_average_of_the_last_six_when_have_decrease_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20))
                    .AddNew(new InvoiceItem(1, 20).WithType(InvoiceItemType.Bill))
                    .AddNew(new InvoiceItem(1, 80).WithType(InvoiceItemType.Investment)),
                new Invoice(new DateTime(2021, 10, 20))
                    .AddNew(new InvoiceItem(1, 10).WithType(InvoiceItemType.PiggyBank))
                    .AddNew(new InvoiceItem(1, 90).WithType(InvoiceItemType.Health)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Leisure)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Market)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Other)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 80).WithType(InvoiceItemType.Health));

            _invoiceOverviewService
                .InvoiceSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(-20);
        }

        [TestMethod]
        public void Should_return_the_difference_in_percent_between_the_total_invoice_cost_this_month_and_the_average_of_the_last_six_when_have_no_change_expense_percent()
        {
            var pastMonthesInvoiceToCompare = new List<Invoice>
            {
                new Invoice(new DateTime(2021, 09, 20))
                    .AddNew(new InvoiceItem(1, 20).WithType(InvoiceItemType.Bill))
                    .AddNew(new InvoiceItem(1, 80).WithType(InvoiceItemType.Investment)),
                new Invoice(new DateTime(2021, 10, 20))
                    .AddNew(new InvoiceItem(1, 10).WithType(InvoiceItemType.PiggyBank))
                    .AddNew(new InvoiceItem(1, 90).WithType(InvoiceItemType.Health)),
                new Invoice(new DateTime(2021, 11, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Leisure)),
                new Invoice(new DateTime(2021, 12, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Market)),
                new Invoice(new DateTime(2022, 01, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Other)),
                new Invoice(new DateTime(2022, 02, 20)).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill))
            };

            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Investment));

            _invoiceOverviewService
                .InvoiceSpentDiffPercent(currentMonthInvoice, pastMonthesInvoiceToCompare)
                .Should().Be(0);
        }

        [TestMethod]
        [DataRow(100, 2/*safe*/)]
        [DataRow(0.01, 2/*safe*/)]
        [DataRow(0, 0/*default*/)]
        [DataRow(-0.01, 1/*danger*/)]
        [DataRow(-300, 1/*danger*/)]
        public void Should_return_the_correct_brief_status(double percent, int status)
        {
            _invoiceOverviewService
                .PercentBriefStatus((decimal)percent)
                .Should().Be((InvoiceBriefStatus)status);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_increase_bill_percent_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(10, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_SIX_MONTHES);

            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(0.01m, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_SIX_MONTHES);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_decrease_bill_percent_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(-500, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_SIX_MONTHES);

            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(-0.01m, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_SIX_MONTHES);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_bill_percent_do_not_change_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(0, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_SIX_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_increase_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoicePercentComparedWithLastSixMonthesText(0.01m, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_SIX_INVOICES);

            _invoiceOverviewService
                .InvoicePercentComparedWithLastSixMonthesText(10, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_SIX_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_decrease_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoicePercentComparedWithLastSixMonthesText(-0.01m, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_SIX_INVOICES);

            _invoiceOverviewService
               .InvoicePercentComparedWithLastSixMonthesText(-20, _localization)
               .Should().Be(_localization.INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_SIX_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_do_not_change_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoicePercentComparedWithLastSixMonthesText(0, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_SIX_INVOICES);
        }
    }
}

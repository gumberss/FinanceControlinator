﻿using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Models.Sync;
using Invoices.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public void Should_return_the_difference_in_percent_between_the_total_invoice_cost_when_there_is_no_invoices_to_compare()
        {
            var currentMonthInvoice = new Invoice(new DateTime(2022, 03, 20))
                .AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Investment));

            _invoiceOverviewService
                .InvoiceSpentDiffPercent(currentMonthInvoice, new List<Invoice>())
                .Should().Be(0);
        }

        [TestMethod]
        [DataRow(100, 2/*safe*/)]
        [DataRow(0.01, 2/*safe*/)]
        [DataRow(0, 0/*default*/)]
        [DataRow(-0.01, 1/*danger*/)]
        [DataRow(-300, 1/*danger*/)]
        public void Should_return_the_correct_brief_status_when_increase_is_better(double percent, int status)
        {
            _invoiceOverviewService
                .PercentBriefStatus((decimal)percent, increaseIsBetter: true)
                .Should().Be((InvoiceBriefStatus)status);
        }

        [TestMethod]
        [DataRow(100, 1/*safe*/)]
        [DataRow(0.01, 1/*safe*/)]
        [DataRow(0, 0/*default*/)]
        [DataRow(-0.01, 2/*danger*/)]
        [DataRow(-300, 2/*danger*/)]
        public void Should_return_the_correct_brief_status_when_decrease_is_better(double percent, int status)
        {
            _invoiceOverviewService
                .PercentBriefStatus((decimal)percent, increaseIsBetter: false)
                .Should().Be((InvoiceBriefStatus)status);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_increase_bill_percent_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(10, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_MONTHES);

            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(0.01m, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_MONTHES);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_decrease_bill_percent_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(-500, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES);

            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(-0.01m, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES);
        }

        [TestMethod]
        public void Should_return_the_correct_bill_percent_text_when_bill_percent_do_not_change_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .BillPercentComparedWithLastSixMonthesText(0, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_increase_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeMonthesText(0.01m, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES);

            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeMonthesText(10, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_decrease_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeMonthesText(-0.01m, _localization)
                .Should().Be(_localization.INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICES);

            _invoiceOverviewService
               .InvoiceCostPercentComparedWithRangeMonthesText(-20, _localization)
               .Should().Be(_localization.INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_invoice_percent_text_when_percent_do_not_change_for_the_last_six_monthes()
        {
            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeMonthesText(0, _localization)
                .Should().Be(_localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES);
        }

        [TestMethod]
        public void Should_return_the_correct_overview_status()
        {
            _invoiceOverviewService.OverviewStatus(InvoiceStatus.Overdue)
                .Should().Be(InvoiceOverviewStatus.Overdue);

            _invoiceOverviewService.OverviewStatus(InvoiceStatus.Paid)
                .Should().Be(InvoiceOverviewStatus.Paid);

            _invoiceOverviewService.OverviewStatus(InvoiceStatus.Closed)
                .Should().Be(InvoiceOverviewStatus.Closed);

            _invoiceOverviewService.OverviewStatus(InvoiceStatus.Open)
                .Should().Be(InvoiceOverviewStatus.Open);

            _invoiceOverviewService.OverviewStatus(InvoiceStatus.Future)
                .Should().Be(InvoiceOverviewStatus.Future);
        }

        [TestMethod]
        public void Should_return_the_correct_overview_status_text()
        {
            _invoiceOverviewService.OverviewStatusText(InvoiceOverviewStatus.Overdue, _localization)
                .Should().Be(_localization.OVERDUE);

            _invoiceOverviewService.OverviewStatusText(InvoiceOverviewStatus.Paid, _localization)
                .Should().Be(_localization.PAID);

            _invoiceOverviewService.OverviewStatusText(InvoiceOverviewStatus.Closed, _localization)
                .Should().Be(_localization.CLOSED);

            _invoiceOverviewService.OverviewStatusText(InvoiceOverviewStatus.Open, _localization)
                .Should().Be(_localization.OPEN);

            _invoiceOverviewService.OverviewStatusText(InvoiceOverviewStatus.Future, _localization)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Should_return_the_invoice_cost_percent_brief_compared_with_no_other_invoices_correctly()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            var localization = new Mock<ILocalization>();

            localization
            .SetupGet(x => x.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES)
            .Returns("HEHE");

            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeBrief(invoice, new List<Invoice>(), new TextParser(), localization.Object)
                .Should().BeEquivalentTo(new InvoiceBrief("HEHE", InvoiceBriefStatus.Default));
        }

        [TestMethod]
        public void Should_return_the_invoice_cost_percent_brief_compared_with_the_invoice_range_correctly()
        {
            var invoice = new Invoice(DateTime.UtcNow).AddNew(new InvoiceItem(1, 150));
            var rangeInvoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow).AddNew(new InvoiceItem(1,100)),
            };

            var localization = new Mock<ILocalization>();
            var textParser = new Mock<ITextParser>();

            localization
                .SetupGet(x => x.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES)
                .Returns("LOCALIZATION");
            textParser
                .Setup(x => x.Parse("LOCALIZATION", It.IsAny<(String key, String value)>(), It.IsAny<(String key, String value)>()))
                .Returns("TEXTPARSER");

            _invoiceOverviewService
                .InvoiceCostPercentComparedWithRangeBrief(invoice, rangeInvoices, textParser.Object, localization.Object)
                .Should().BeEquivalentTo(new InvoiceBrief("TEXTPARSER", InvoiceBriefStatus.Danger));

            textParser.Verify(x =>
                x.Parse("LOCALIZATION",
                    It.Is<(String key, String value)>(k => k.key == "PERCENT"),
                    It.Is<(String key, String value)>(k => k.key == "INVOICES_QUANTITIES")), Times.Once);
        }

        [TestMethod]
        public void Should_return_the_bill_cost_percent_brief_compared_with_the_invoice_range_correctly()
        {
            var invoice = new Invoice(DateTime.UtcNow).AddNew(new InvoiceItem(1, 50).WithType(InvoiceItemType.Bill));
            var rangeInvoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow).AddNew(new InvoiceItem(1, 100).WithType(InvoiceItemType.Bill)),
            };

            var localization = new Mock<ILocalization>();
            var textParser = new Mock<ITextParser>();

            localization
                .SetupGet(x => x.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES)
                .Returns("LOCALIZATION");
            textParser
                .Setup(x => x.Parse("LOCALIZATION", It.IsAny<(String key, String value)>(), It.IsAny<(String key, String value)>()))
                .Returns("TEXTPARSER");

            _invoiceOverviewService
                .BillCostPercentComparedWithRangeMonthesBrief(invoice, rangeInvoices, textParser.Object, localization.Object)
                .Should().BeEquivalentTo(new InvoiceBrief("TEXTPARSER", InvoiceBriefStatus.Safe));

            textParser.Verify(x =>
                x.Parse("LOCALIZATION",
                    It.Is<(String key, String value)>(k => k.key == "PERCENT"),
                    It.Is<(String key, String value)>(k => k.key == "INVOICES_QUANTITIES")), Times.Once);
        }

        [TestMethod]
        public void Should_return_investment_percent_brief_compared_with_the_invoice_range_correctly()
        {
            var invoice = new Invoice(DateTime.UtcNow)
                .AddNew(new InvoiceItem(1, 50).WithType(InvoiceItemType.Investment));

            var localization = new Mock<ILocalization>();
            var textParser = new Mock<ITextParser>();

            localization
                .SetupGet(x => x.INVOICE_OVERVIEW_INVESTMENT_PERCENT)
                .Returns("LOCALIZATION");
            textParser
                .Setup(x => x.Parse("LOCALIZATION", It.IsAny<(String key, String value)>()))
                .Returns("TEXTPARSER");

            _invoiceOverviewService
                .InvestmentPercentBrief(invoice, textParser.Object, localization.Object)
                .Should().BeEquivalentTo(new InvoiceBrief("TEXTPARSER", InvoiceBriefStatus.Safe));

            textParser.Verify(x =>
                x.Parse("LOCALIZATION",
                    It.Is<(String key, String value)>(k => k.key == "PERCENT")), Times.Once);
        }

        [TestMethod]
        public void Should_return_future_purchase_percent_brief_compared_with_the_invoice_range_correctly()
        {
            var invoice = new Invoice(DateTime.UtcNow)
                .AddNew(new InvoiceItem(1, 50).WithType(InvoiceItemType.PiggyBank));

            var localization = new Mock<ILocalization>();
            var textParser = new Mock<ITextParser>();

            localization
                .SetupGet(x => x.OVERVIEW_FUTURE_PURCHASE_PERCENT)
                .Returns("LOCALIZATION");
            textParser
                .Setup(x => x.Parse("LOCALIZATION", It.IsAny<(String key, String value)>()))
                .Returns("TEXTPARSER");

            _invoiceOverviewService
                .FuturePurchasePercentBrief(invoice, textParser.Object, localization.Object)
                .Should().BeEquivalentTo(new InvoiceBrief("TEXTPARSER", InvoiceBriefStatus.Safe));

            textParser.Verify(x =>
                x.Parse("LOCALIZATION",
                    It.Is<(String key, String value)>(k => k.key == "PERCENT")), Times.Once);
        }

        [TestMethod]
        public void Should_build_partitions_correctly_when_there_is_no_invoice_item_created()
        {
            var localization = new Mock<ILocalization>();

            var partitions = _invoiceOverviewService.BuildPartitions(new List<InvoiceItem>(), localization.Object);

            var invoiceItemTypesCount = Enum.GetValues(typeof(InvoiceItemType)).Length;

            partitions.Count.Should().Be(invoiceItemTypesCount);
            partitions.All(x => x.TotalValue == 0).Should().BeTrue();
            partitions.All(x => x.Percent == 0).Should().BeTrue();

            Enum.GetValues(typeof(InvoiceItemType))
                .Cast<InvoiceItemType>()
                .ToList()
                .ForEach(invoiceType => localization.Verify(x => x.INVOICE_ITEM_TYPE(invoiceType), Times.Once));
        }

        [TestMethod]
        public void Should_build_partitions_correctly_when_there_is_some_invoice_items_created()
        {
            var localization = new Mock<ILocalization>();

            var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem(1, 33).WithType(InvoiceItemType.Investment),
                new InvoiceItem(1, 33).WithType(InvoiceItemType.PiggyBank),
                new InvoiceItem(1, 34).WithType(InvoiceItemType.Bill)
            };

            var partitions = _invoiceOverviewService.BuildPartitions(invoiceItems, localization.Object);

            var invoiceItemTypesCount = Enum.GetValues(typeof(InvoiceItemType)).Length;

            partitions.Count.Should().Be(invoiceItemTypesCount);

            var valuablePartitions = partitions
                .Where(x => x.TotalValue > 0)
                ;

            valuablePartitions
                .Select(x => x.Type)
                .Should()
                .BeEquivalentTo(new[] { InvoiceItemType.Investment, InvoiceItemType.PiggyBank, InvoiceItemType.Bill });

            valuablePartitions
                .Select(x => x.Percent)
                .Should().BeEquivalentTo(new[] { 33, 33, 34 });

            Enum.GetValues(typeof(InvoiceItemType))
                .Cast<InvoiceItemType>()
                .ToList()
                .ForEach(invoiceType => localization.Verify(x => x.INVOICE_ITEM_TYPE(invoiceType), Times.Once));
        }
    }
}

using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Invoices.Tests.Application.AppServices
{
    [TestClass]
    [JourneyCategory(TestUserJourneyEnum.Overview)]
    [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.Overview)]
    public class InvoiceSyncAppServiceTest
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepository;
        private readonly Mock<ILocalization> _localization;
        private readonly Mock<ITextParser> _textParser;
        private readonly Mock<IInvoiceService> _invoiceService;
        private readonly Mock<ILogger<IInvoiceAppService>> _logger;
        private readonly Mock<IDateService> _dateService;
        private readonly Mock<IInvoiceOverviewService> _invoiceOverviewService;
        private readonly InvoiceSyncAppService _invoiceSyncAppService;

        List<Invoice> invoicesDb = new List<Invoice>
        {
            new Invoice(DateTime.UtcNow.AddMonths(2)).AddNew(new InvoiceItem(1, 10)),
            new Invoice(DateTime.UtcNow.AddMonths(1)).AddNew(new InvoiceItem(1, 15)),
            new Invoice(DateTime.UtcNow).AddNew(new InvoiceItem(1, 20)),
            new Invoice(DateTime.UtcNow.AddMonths(-1)).AddNew(new InvoiceItem(1, 5)),
            new Invoice(DateTime.UtcNow.AddMonths(-2)).AddNew(new InvoiceItem(1, 12)),
            new Invoice(DateTime.UtcNow.AddMonths(-3)).AddNew(new InvoiceItem(1, 2)),
            new Invoice(DateTime.UtcNow.AddMonths(-4)).AddNew(new InvoiceItem(1, 2)),
            new Invoice(DateTime.UtcNow.AddMonths(-5)).AddNew(new InvoiceItem(1, 1)),
            new Invoice(DateTime.UtcNow.AddMonths(-6)).AddNew(new InvoiceItem(1, 13)),
            new Invoice(DateTime.UtcNow.AddMonths(-7)).AddNew(new InvoiceItem(1, 20)),
        };

        public InvoiceSyncAppServiceTest()
        {
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _localization = new Mock<ILocalization>();
            _textParser = new Mock<ITextParser>();
            _invoiceService = new Mock<IInvoiceService>();
            _logger = new Mock<ILogger<IInvoiceAppService>>();
            _dateService = new Mock<IDateService>();
            _invoiceOverviewService = new Mock<IInvoiceOverviewService>();

            _invoiceSyncAppService = new InvoiceSyncAppService(
                _invoiceRepository.Object,
                _localization.Object,
                _textParser.Object,
                _invoiceService.Object,
                _logger.Object,
                _dateService.Object,
                _invoiceOverviewService.Object);

            var invoiceService = new InvoiceService();
            var invoiceOverviewService = new InvoiceOverviewService();

            _invoiceService.Setup(x => x.AnyChangeSince(It.IsAny<DateTime>()))
                .Returns(invoiceService.AnyChangeSince);
            _invoiceService.Setup(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()))
                .Returns(invoiceService.AnyChangeSince);
            _invoiceService.Setup(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(invoiceService.StatusChanged);
            _invoiceService.Setup(x => x.LastInvoicesFrom(It.IsAny<Invoice>(), It.IsAny<List<Invoice>>(), 6))
                .Returns(invoiceService.LastInvoicesFrom);

            _invoiceOverviewService.Setup(x => x.FuturePurchasePercentBrief(
                 It.IsAny<Invoice>(), It.IsAny<ITextParser>(), It.IsAny<ILocalization>()))
                 .Returns(invoiceOverviewService.FuturePurchasePercentBrief);

            _invoiceOverviewService.Setup(x => x.InvestmentPercentBrief(
                It.IsAny<Invoice>(), It.IsAny<ITextParser>(), It.IsAny<ILocalization>()))
                .Returns(invoiceOverviewService.InvestmentPercentBrief);

            _invoiceOverviewService.Setup(x => x.BillCostPercentComparedWithRangeMonthesBrief(
                It.IsAny<Invoice>(), It.IsAny<List<Invoice>>(), It.IsAny<ITextParser>(), It.IsAny<ILocalization>()))
                .Returns(invoiceOverviewService.BillCostPercentComparedWithRangeMonthesBrief);

            _invoiceOverviewService.Setup(x => x.InvoiceCostPercentComparedWithRangeBrief(
                It.IsAny<Invoice>(), It.IsAny<List<Invoice>>(), It.IsAny<ITextParser>(), It.IsAny<ILocalization>()))
                .Returns(invoiceOverviewService.InvoiceCostPercentComparedWithRangeBrief);

            _dateService
             .Setup(x => x.FirstMonthDayDate(It.IsAny<DateTime>()))
             .Returns(new DateService().FirstMonthDayDate);

            _invoiceRepository
               .Setup(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()))
               .Returns<Expression<Func<Invoice, object>>, Expression<Func<Invoice, bool>>[]>((include, wheres)
                   => Result.Try(() =>
                   {
                       var invs = invoicesDb.Select(x => x);
                       wheres.ToList().ForEach(x => invs = invs.Where(x.Compile()).ToList());
                       return invs.ToList();
                   }));
        }

        [TestMethod]
        public void Should_sync_when_customer_has_items_with_changes_on_the_server()
        {
            var lastSyncDate = DateTime.UtcNow.AddYears(-1);
            var lastSyncDateLong = ((DateTimeOffset)lastSyncDate).ToUnixTimeMilliseconds();

            var result = _invoiceSyncAppService.SyncUpdatesFrom(lastSyncDateLong).Result;

            _invoiceService.Verify(x => x.AnyChangeSince(It.IsAny<DateTime>()), Times.Exactly(11), "One time for each invoice plus one time to query from database");
            _invoiceService.Verify(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never, "All the invoices have changes (AnyChangeSince)");
            _invoiceService.Verify(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()), Times.Once);
            _invoiceRepository
                .Verify(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()), Times.Once);
            _dateService.Verify(x => x.FirstMonthDayDate(It.IsAny<DateTime>()), Times.Once);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.MonthDataSyncs.Should().HaveCount(10);
            result.Value.MonthDataSyncs.Where(x => x.Overview.Briefs.Count == 4)
                .Should().HaveCount(9, because: "All the briefs should be returned but he last one");
            result.Value.MonthDataSyncs.Where(x => x.Overview.Briefs.Count == 2)
                .Should().HaveCount(1, because: "The old one that do not have invoices to compare");
        }

        [TestMethod]
        public void Should_sync_when_customer_has_no_items_with_changes_on_the_server()
        {
            var lastSyncDate = DateTime.UtcNow.AddYears(100);
            var lastSyncDateLong = ((DateTimeOffset)lastSyncDate).ToUnixTimeMilliseconds();

            var result = _invoiceSyncAppService.SyncUpdatesFrom(lastSyncDateLong).Result;

            _invoiceService.Verify(x => x.AnyChangeSince(It.IsAny<DateTime>()), Times.Once, "No one invoice will be returned from database");
            _invoiceService.Verify(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()), Times.Once);
            _invoiceService.Verify(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _invoiceRepository
                .Verify(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()), Times.Once);
            _dateService.Verify(x => x.FirstMonthDayDate(It.IsAny<DateTime>()), Times.Once);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.MonthDataSyncs.Should().HaveCount(0);
        }

        [TestMethod]
        public void Should_return_an_error_when_something_wrong_happens_when_trying_to_retrieve_data_from_database()
        {
            _invoiceRepository
              .Setup(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()))
              .ReturnsAsync(new BusinessException(HttpStatusCode.InternalServerError, "Error"));

            var lastSyncDate = DateTime.UtcNow.AddYears(100);
            var lastSyncDateLong = ((DateTimeOffset)lastSyncDate).ToUnixTimeMilliseconds();

            var result = _invoiceSyncAppService.SyncUpdatesFrom(lastSyncDateLong).Result;

            _invoiceService.Verify(x => x.AnyChangeSince(It.IsAny<DateTime>()), Times.Once, "No one invoice will be returned from database");
            _invoiceService.Verify(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()), Times.Once);
            _invoiceService.Verify(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _invoiceRepository
                .Verify(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()), Times.Once);
            _dateService.Verify(x => x.FirstMonthDayDate(It.IsAny<DateTime>()), Times.Once);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be(HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void Should_not_return_briefs_that_compare_with_range_when_there_is_no_invoices_to_compare_with()
        {
            _invoiceService.Setup(x => x.LastInvoicesFrom(It.IsAny<Invoice>(), It.IsAny<List<Invoice>>(), 6))
              .Returns(new List<Invoice>());

            var lastSyncDate = DateTime.UtcNow.AddYears(-1);
            var lastSyncDateLong = ((DateTimeOffset)lastSyncDate).ToUnixTimeMilliseconds();

            var result = _invoiceSyncAppService.SyncUpdatesFrom(lastSyncDateLong).Result;

            _invoiceService.Verify(x => x.AnyChangeSince(It.IsAny<DateTime>()), Times.Exactly(11), "One time for each invoice plus one time to query from database");
            _invoiceService.Verify(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never, "All the invoices have changes (AnyChangeSince)");
            _invoiceService.Verify(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()), Times.Once);
            _invoiceRepository
                .Verify(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()), Times.Once);
            _dateService.Verify(x => x.FirstMonthDayDate(It.IsAny<DateTime>()), Times.Once);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.MonthDataSyncs.Should().HaveCount(10);
            result.Value.MonthDataSyncs.All(x => x.Overview.Briefs.Count == 2)
                .Should().BeTrue(because: "The briefs that are compared with monthes need to have monthes to compare");
        }

        [TestMethod]
        public void Should_sync_when_the_invoices_change_the_state()
        {
            _invoiceService.Setup(x => x.LastInvoicesFrom(It.IsAny<Invoice>(), It.IsAny<List<Invoice>>(), 6))
              .Returns(new List<Invoice>());

            var lastSyncDate = DateTime.UtcNow.AddMonths(1);
            var lastSyncDateLong = ((DateTimeOffset)lastSyncDate).ToUnixTimeMilliseconds();

            var result = _invoiceSyncAppService.SyncUpdatesFrom(lastSyncDateLong).Result;

            _invoiceService.Verify(x => x.AnyChangeSince(It.IsAny<DateTime>()), Times.Exactly(11), "One time for each invoice plus one time to query from database");
            _invoiceService.Verify(x => x.StatusChanged(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Exactly(10), "No one invoice have changes");
            _invoiceService.Verify(x => x.ClosedInvoiceAfter(It.IsAny<DateTime>()), Times.Once);
            _invoiceRepository
                .Verify(x => x.GetAllAsync(x => x.Items, It.IsAny<Expression<Func<Invoice, bool>>>()), Times.Once);
            _dateService.Verify(x => x.FirstMonthDayDate(It.IsAny<DateTime>()), Times.Once);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.MonthDataSyncs.Should().HaveCount(2, because: "One was closed and one is open now");
        }
    }
}

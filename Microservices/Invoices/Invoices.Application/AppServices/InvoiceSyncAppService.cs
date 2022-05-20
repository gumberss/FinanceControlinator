using Invoices.Application.Interfaces.AppServices;
using FinanceControlinator.Common.Utils;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Models.Sync;
using Invoices.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Parsers.TextParsers;
using Invoices.Data.Commons;

namespace Invoices.Application.AppServices
{
    public class InvoiceSyncAppService : IInvoiceSyncAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IInvoiceAppService> _logger;
        private readonly IInvoiceOverviewService _invoiceOverviewService;
        private readonly IInvoiceService _invoiceService;
        private readonly IDateService _dateService;
        private readonly ITextParser _textParser;

        public InvoiceSyncAppService(
                IInvoiceRepository invoiceRepository
                , ILocalization localization
                , ITextParser textParser
                , IInvoiceService invoiceService
                , ILogger<IInvoiceAppService> logger
                , IDateService dateService
                , IInvoiceOverviewService invoiceOverviewService
            )
        {
            _invoiceRepository = invoiceRepository;
            _localization = localization;
            _logger = logger;
            _invoiceOverviewService = invoiceOverviewService;
            _invoiceService = invoiceService;
            _dateService = dateService;
            _textParser = textParser;
        }

        public async Task<Result<InvoiceSync, BusinessException>> SyncUpdatesFrom(long lastSyncTimestamp)
        {
            var lastSyncDateTime = DateTimeOffset.FromUnixTimeMilliseconds(lastSyncTimestamp).LocalDateTime;
            long currentSyncDate = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();

            var monthesToCompare = 6;
            var invoicesContextStartDate = _dateService.FirstMonthDayDate(lastSyncDateTime.AddMonths(-monthesToCompare));

            //filter by user (other task)
            var contextInvoices = await _invoiceRepository.GetAllAsync(x => x.Items
            , _invoiceService.AnyChangeSince(lastSyncDateTime)
            .Or(_invoiceService.ClosedInvoiceAfter(invoicesContextStartDate)));

            if (contextInvoices.IsFailure) return contextInvoices.Error;

            var updatedInvoices = contextInvoices.Value
                .Where(_invoiceService.AnyChangeSince(lastSyncDateTime));

            return new InvoiceSync(
                syncDate: currentSyncDate,
                monthDataSyncs: updatedInvoices
                    .Select(invoice => BuildMonthDataSync(invoice, contextInvoices))
                    .ToList());
        }

        private InvoiceMonthDataSync BuildMonthDataSync(Invoice invoice, List<Invoice> contextInvoices)
        {
            var baseDate = DateTime.Now;
            var overviewStatus = _invoiceOverviewService.OverviewStatus(_invoiceService.Status(invoice, baseDate));

            var overviewStatusText = _textParser.Parse(
                _invoiceOverviewService.OverviewStatusText(overviewStatus, _localization),
                ("DAYS", _invoiceService.DaysRemainingToNextStage(invoice, baseDate).ToString()));

            var overview = new InvoiceOverviewSync(
                 date: _invoiceOverviewService.InvoiceCloseDateText(invoice),
                 statusText: overviewStatusText,
                 status: overviewStatus,
                 totalCost: _localization.FORMAT_MONEY(invoice.TotalCost),
                 briefs: BuildBriefs(invoice, contextInvoices).ToList(),
                 partitions: BuildPartitions(invoice));

            return new InvoiceMonthDataSync(overview, invoice);
        }

        private List<InvoicePartition> BuildPartitions(Invoice invoice)
        {
            return null;
        }

        private IEnumerable<InvoiceBrief> BuildBriefs(Invoice invoice, List<Invoice> contextInvoices)
        {
            yield return new InvoiceBrief(_textParser.Parse(_localization.OVERVIEW_FUTURE_PURCHASE_PERCENT
                , ("PERCENT", _invoiceOverviewService.FuturePurchasePercent(invoice).ToString(_localization.CULTURE))));

            yield return new InvoiceBrief(_textParser.Parse(_localization.INVOICE_OVERVIEW_INVESTMENT_PERCENT
                , ("PERCENT", _invoiceOverviewService.InvestmentPercent(invoice).ToString(_localization.CULTURE))));

            var lastSixInvoicesFromCurrentInvoce = _invoiceService.LastInvoicesFrom(invoice, contextInvoices, 6);

            var billPercentLastSixMonthes = _invoiceOverviewService
                    .BillAverageSpentDiffPercent(invoice, lastSixInvoicesFromCurrentInvoce);

            yield return new InvoiceBrief(
                _textParser.Parse(
                    _invoiceOverviewService.BillPercentComparedWithLastSixMonthesText(billPercentLastSixMonthes, _localization)
                    , ("PERCENT", Math.Abs(billPercentLastSixMonthes).ToString(_localization.CULTURE)))
                , _invoiceOverviewService.PercentBriefStatus(billPercentLastSixMonthes));

            var invoiceCosDifftPercentLastSixMonthes = _invoiceOverviewService
                  .InvoiceSpentDiffPercent(invoice, lastSixInvoicesFromCurrentInvoce);

            yield return new InvoiceBrief(
                _textParser.Parse(
                    _invoiceOverviewService.InvoicePercentComparedWithLastSixMonthesText(invoiceCosDifftPercentLastSixMonthes, _localization)
                    , ("PERCENT", invoiceCosDifftPercentLastSixMonthes.ToString(_localization.CULTURE)))
                , _invoiceOverviewService.PercentBriefStatus(invoiceCosDifftPercentLastSixMonthes));
        }
    }
}

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
            DateTime currentSyncDate = DateTime.UtcNow;

            var monthesToCompare = 6;
            var invoicesContextStartDate = _dateService.FirstMonthDayDate(lastSyncDateTime.AddMonths(-monthesToCompare));

            //filter by user (other task)
            var contextInvoices = await _invoiceRepository.GetAllAsync(x => x.Items
            , _invoiceService.AnyChangeSince(lastSyncDateTime)
            .Or(_invoiceService.ClosedInvoiceAfter(invoicesContextStartDate)));

            if (contextInvoices.IsFailure) return contextInvoices.Error;

            var updatedInvoices = contextInvoices.Value
                .Where(invoice => _invoiceService.AnyChangeSince(lastSyncDateTime)(invoice)
                || _invoiceService.StatusChanged(lastSyncDateTime, currentSyncDate)(invoice));

            return new InvoiceSync(
                syncDate: ((DateTimeOffset)currentSyncDate).ToUnixTimeMilliseconds(),
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
                 briefs: BuildBriefs(invoice, contextInvoices),
                 partitions: _invoiceOverviewService.BuildPartitions(invoice.Items, _localization));

            return new InvoiceMonthDataSync(overview, invoice);
        }

        private List<InvoiceBrief> BuildBriefs(Invoice invoice, List<Invoice> contextInvoices)
        {
            var lastSixInvoicesFromCurrentInvoce = _invoiceService.LastInvoicesFrom(invoice, contextInvoices, 6);

            return new List<InvoiceBrief>
            {
                _invoiceOverviewService.FuturePurchasePercentBrief(invoice, _textParser, _localization),
                _invoiceOverviewService.InvestmentPercentBrief(invoice, _textParser, _localization),
                _invoiceOverviewService.BillPercentComparedWithRangeMonthesBrief(invoice, lastSixInvoicesFromCurrentInvoce, _textParser, _localization),
                _invoiceOverviewService.InvoicePercentComparedWithLastSixMonthesBrief(invoice, lastSixInvoicesFromCurrentInvoce, _textParser, _localization)
            };
        }
    }
}

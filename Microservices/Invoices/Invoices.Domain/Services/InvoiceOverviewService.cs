﻿using FinanceControlinator.Common.Parsers.TextParsers;
using Invoices.Domain.Enums;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Models.Sync;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Services
{
    public interface IInvoiceOverviewService
    {
        string InvoiceCloseDateText(Invoice invoice);
        decimal FuturePurchasePercent(Invoice invoice);
        decimal InvestmentPercent(Invoice invoice);
        decimal BillAverageSpentDiffPercent(Invoice current, List<Invoice> comparable);
        decimal InvoiceSpentDiffPercent(Invoice current, List<Invoice> comparable);
        InvoiceBriefStatus PercentBriefStatus(decimal percentIncrease);
        string BillPercentComparedWithLastSixMonthesText(decimal billPercentIncrease, ILocalization localization);
        string InvoicePercentComparedWithLastSixMonthesText(decimal invoiceCosDifftPercentLastSixMonthes, ILocalization localization);
        InvoiceOverviewStatus OverviewStatus(InvoiceStatus invoiceStatus);
        string OverviewStatusText(InvoiceOverviewStatus overviewStatus, ILocalization localization);
        List<InvoicePartition> BuildPartitions(List<InvoiceItem> invoiceItems, ILocalization localization);

        InvoiceBrief InvoicePercentComparedWithLastSixMonthesBrief(Invoice invoice, List<Invoice> rangeInvoicesFromCurrentInvoice, ITextParser textParser, ILocalization localization);
        InvoiceBrief BillPercentComparedWithRangeMonthesBrief(Invoice invoice, List<Invoice> rangeInvoicesFromCurrentInvoice, ITextParser textParser, ILocalization localization);
        InvoiceBrief InvestmentPercentBrief(Invoice invoice, ITextParser textParser, ILocalization localization);
        InvoiceBrief FuturePurchasePercentBrief(Invoice invoice, ITextParser textParser, ILocalization localization);
    }

    public class InvoiceOverviewService : IInvoiceOverviewService
    {
        public string InvoiceCloseDateText(Invoice invoice)
            => invoice.CloseDate.ToString("MM/yyyy");

        public decimal FuturePurchasePercent(Invoice invoice)
            => PercentByType(invoice, InvoiceItemType.PiggyBank);

        public decimal InvestmentPercent(Invoice invoice)
            => PercentByType(invoice, InvoiceItemType.Investment);

        public decimal BillAverageSpentDiffPercent(Invoice current, List<Invoice> comparable)
            => AveragePercent(current.Items
                    .Where(x => x.Type == InvoiceItemType.Bill)
                    .Sum(x => x.InstallmentCost)
                , comparable
                    .SelectMany(x => x.Items)
                    .Where(x => x.Type == InvoiceItemType.Bill)
                    .Sum(x => x.InstallmentCost)
                , comparable.Count);

        public decimal InvoiceSpentDiffPercent(Invoice current, List<Invoice> comparable)
            => AveragePercent(current.TotalCost, comparable.Sum(x => x.TotalCost), comparable.Count);

        public InvoiceBriefStatus PercentBriefStatus(decimal percentIncrease)
          => percentIncrease switch
          {
              > 0 => InvoiceBriefStatus.Safe,
              < 0 => InvoiceBriefStatus.Danger,
              _ => InvoiceBriefStatus.Default
          };

        public string BillPercentComparedWithLastSixMonthesText(decimal billPercentIncrease, ILocalization localization)
            => billPercentIncrease switch
            {
                > 0 => localization.INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_MONTHES,
                < 0 => localization.INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES,
                _ => localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES
            };

        public string InvoicePercentComparedWithLastSixMonthesText(decimal invoiceCostDifftPercentLastSixMonthes, ILocalization localization)
            => invoiceCostDifftPercentLastSixMonthes switch
            {
                > 0 => localization.INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES,
                < 0 => localization.INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICES,
                _ => localization.INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES
            };

        private static decimal PercentByType(Invoice invoice, InvoiceItemType type)
            => Percent(invoice.Items
                    .Where(x => x.Type == type)
                    .Sum(x => x.InstallmentCost)
                , invoice.TotalCost);

        public InvoiceOverviewStatus OverviewStatus(InvoiceStatus invoiceStatus)
            => invoiceStatus switch
            {
                InvoiceStatus.Overdue => InvoiceOverviewStatus.Overdue,
                InvoiceStatus.Paid => InvoiceOverviewStatus.Paid,
                InvoiceStatus.Closed => InvoiceOverviewStatus.Closed,
                InvoiceStatus.Open => InvoiceOverviewStatus.Open,
                _ => InvoiceOverviewStatus.Future,
            };

        public string OverviewStatusText(InvoiceOverviewStatus overviewStatus, ILocalization localization)
            => overviewStatus switch
            {
                InvoiceOverviewStatus.Overdue => localization.OVERDUE,
                InvoiceOverviewStatus.Paid => localization.PAID,
                InvoiceOverviewStatus.Open => localization.OPEN,
                InvoiceOverviewStatus.Closed => localization.CLOSED,
                _ => string.Empty,
            };

        public List<InvoicePartition> BuildPartitions(List<InvoiceItem> invoiceItems, ILocalization localization)
        {
            var totalSpent = invoiceItems.Sum(x => x.InstallmentCost);

            var partitionsSpent = invoiceItems
                .GroupBy(x => x.Type)
                .Select(x =>
                    new InvoicePartition(type: x.Key,
                        typeText: localization.INVOICE_ITEM_TYPE(x.Key),
                        percent: Percent(x.Sum(y => y.InstallmentCost), totalSpent),
                        totalValue: x.Sum(y => y.InstallmentCost)))
                .ToList();

            var partitionsNotSpent =
                Enum.GetValues(typeof(InvoiceItemType))
                .Cast<InvoiceItemType>()
                .Except(partitionsSpent.Select(x => x.Type))
                .Select(x => new InvoicePartition(x, localization.INVOICE_ITEM_TYPE(x), 0, 0));

            return partitionsSpent.Concat(partitionsNotSpent).ToList();
        }

        public InvoiceBrief InvoicePercentComparedWithLastSixMonthesBrief(Invoice invoice, List<Invoice> rangeInvoicesFromCurrentInvoice, ITextParser textParser, ILocalization localization)
        {
            var invoiceCosDiffPercentRangeMonthes = InvoiceSpentDiffPercent(invoice, rangeInvoicesFromCurrentInvoice);

            return new InvoiceBrief(
                textParser.Parse(
                    InvoicePercentComparedWithLastSixMonthesText(invoiceCosDiffPercentRangeMonthes, localization)
                    , ("PERCENT", invoiceCosDiffPercentRangeMonthes.ToString("F", localization.CULTURE))
                    , ("INVOICES_QUANTITIES", rangeInvoicesFromCurrentInvoice.Count.ToString()))
                , PercentBriefStatus(invoiceCosDiffPercentRangeMonthes));
        }

        public InvoiceBrief BillPercentComparedWithRangeMonthesBrief(Invoice invoice, List<Invoice> rangeInvoicesFromCurrentInvoice, ITextParser textParser, ILocalization localization)
        {
            var billPercentRangeMonthes = BillAverageSpentDiffPercent(invoice, rangeInvoicesFromCurrentInvoice);

            return new InvoiceBrief(
                textParser.Parse(
                    BillPercentComparedWithLastSixMonthesText(billPercentRangeMonthes, localization)
                    , ("PERCENT", Math.Abs(billPercentRangeMonthes).ToString("F", localization.CULTURE))
                    , ("INVOICES_QUANTITIES", rangeInvoicesFromCurrentInvoice.Count.ToString()))
                , PercentBriefStatus(billPercentRangeMonthes));
        }

        public InvoiceBrief InvestmentPercentBrief(Invoice invoice, ITextParser textParser, ILocalization localization)
            => new InvoiceBrief(textParser.Parse(localization.INVOICE_OVERVIEW_INVESTMENT_PERCENT
                , ("PERCENT", InvestmentPercent(invoice).ToString("F", localization.CULTURE))));

        public InvoiceBrief FuturePurchasePercentBrief(Invoice invoice, ITextParser textParser, ILocalization localization)
            => new InvoiceBrief(textParser.Parse(localization.OVERVIEW_FUTURE_PURCHASE_PERCENT
                , ("PERCENT", FuturePurchasePercent(invoice).ToString("F", localization.CULTURE))));

        private static decimal AveragePercent(decimal toFind, decimal total, int totalItems)
         => totalItems == 0
         ? 0
         : Percent(toFind - total / totalItems, total / totalItems);

        private static decimal Percent(decimal toFind, decimal total)
            => total == 0
            ? 0
            : (toFind / total) * 100;
    }
}

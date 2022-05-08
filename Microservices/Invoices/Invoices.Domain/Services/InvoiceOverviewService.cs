using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Services
{
    public interface IInvoiceOverviewService
    {

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

        private static decimal PercentByType(Invoice invoice, InvoiceItemType type)
            => Percent(invoice.Items
                    .Where(x => x.Type == type)
                    .Sum(x => x.InstallmentCost)
                , invoice.TotalCost);

        private static decimal AveragePercent(decimal toFind, decimal total, int totalItems)
            => Percent(toFind - total / totalItems, total / totalItems);

        private static decimal Percent(decimal toFind, decimal total)
            => (toFind / total) * 100;

    }
}

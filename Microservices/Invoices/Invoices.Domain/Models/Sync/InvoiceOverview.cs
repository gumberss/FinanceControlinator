using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public class InvoiceOverview
    {
        public string Date { get; set; }

        public string StatusText { get; set; }

        public InvoiceOverviewStatus Status { get; set; }

        public string TotalCost { get; set; }

        public List<InvoiceBrief> Briefs { get; set; }

        public List<InvoicePartition> Partitions { get; set; }

        public InvoiceOverview(string date, 
            string statusText, 
            InvoiceOverviewStatus status, 
            string totalCost, 
            List<InvoiceBrief> briefs, 
            List<InvoicePartition> partitions)
        {
            Date = date;
            StatusText = statusText;
            Status = status;
            TotalCost = totalCost;
            Briefs = briefs;
            Partitions = partitions;
        }
    }

    public enum InvoiceOverviewStatus
    {
        Overdue = 0,
        Paid = 1,
        Closed = 2,
        Next = 3,
        Open = 4
    }
}

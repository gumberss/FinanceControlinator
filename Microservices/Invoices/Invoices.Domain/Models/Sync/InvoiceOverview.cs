using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public class InvoiceOverview
    {
        public string Date { get; set; }

        public string Status { get; set; }

        public string TotalCost { get; set; }

        public List<InvoiceBrief> Briefs { get; set; }

        public List<InvoicePartition> Partitions { get; set; }

        public InvoiceOverview(List<InvoiceBrief> briefs, List<InvoicePartition> partitions)
        {
            Briefs = briefs;
            Partitions = partitions;
        }
    }
}

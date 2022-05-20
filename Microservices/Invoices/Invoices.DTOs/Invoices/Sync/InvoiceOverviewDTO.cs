using Invoices.DTOs.Invoices.Enum;
using System.Collections.Generic;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceOverviewDTO
    {
        public string Date { get; set; }

        public string StatusText { get; set; }

        public InvoiceOverviewStatus Status { get; set; }

        public string TotalCost { get; set; }

        public List<InvoiceBriefDTO> Briefs { get; set; }

        public List<InvoicePartitionDTO> Partitions { get; set; }
    }
}

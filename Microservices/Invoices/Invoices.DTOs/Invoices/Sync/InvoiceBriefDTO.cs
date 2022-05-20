using Invoices.DTOs.Invoices.Enum;
using System;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceBriefDTO
    {
        public String Text { get; set; }
        public InvoiceBriefStatus Status { get; set; }
    }
}

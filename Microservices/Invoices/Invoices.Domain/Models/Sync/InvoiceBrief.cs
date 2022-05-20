using System;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceBrief
    {
        public InvoiceBrief(String text, InvoiceBriefStatus status = InvoiceBriefStatus.Default)
        {
            Text = text;
            Status = status;
        }

        public String Text { get; set; }
        public InvoiceBriefStatus Status { get; set; }
    }

    public enum InvoiceBriefStatus
    {
        Default = 0,
        Danger = 1,
        Safe = 2,
    }
}

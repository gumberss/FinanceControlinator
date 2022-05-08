using System;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceBrief
    {
        public InvoiceBrief(String text)
        {
            Text = text;
        }

        public String Text { get; set; }
    }
}

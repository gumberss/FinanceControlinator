using Invoices.Domain.Enums;

namespace Invoices.Domain.Models.Sync
{
    public record InvoicePartition
    {
        public InvoicePartition(InvoiceItemType type, float percent, decimal totalValue)
        {
            Type = type;
            Percent = percent;
            TotalValue = totalValue;
        }

        public InvoiceItemType Type { get; set; }

        public float Percent { get; set; }

        public decimal TotalValue { get; set; }
    }
}

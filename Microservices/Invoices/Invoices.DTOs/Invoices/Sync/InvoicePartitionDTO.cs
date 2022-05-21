using Invoices.DTOs.Invoices.Enum;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoicePartitionDTO
    {
        public InvoiceItemType Type { get; set; }

        public float Percent { get; set; }

        public string TypeText { get; set; }

        public decimal TotalValue { get; set; }
    }
}

using Invoices.Domain.Enums;

namespace Invoices.Domain.Models.Sync
{
    public record InvoicePartition
    {
        public InvoicePartition(InvoiceItemType type, string typeText, decimal percent, decimal totalValue)
        {
            Type = type;
            Percent = percent;
            TypeText = typeText;
            TotalValue = totalValue;
        }

        public InvoiceItemType Type { get; set; }
        public decimal Percent { get; set; }
        public string TypeText { get; set; }
        public decimal TotalValue { get; set; }
    }
}

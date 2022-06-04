using Invoices.DTOs.Invoices.Enum;
using System;

namespace Invoices.DTOs.Invoices
{
    public record InvoiceItemDTO
    {
        public String Id { get; set; }

        public String ExpenseId { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal InstallmentCost { get; set; }

        public InvoiceItemType Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public String Location { get; set; }

        public String Title { get; set; }

        public String DetailsPath { get; set; }
    }
}

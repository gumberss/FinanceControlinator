using Invoices.Domain.Enums;
using System;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceItemSync
    {
        public InvoiceItemSync(string id, 
            string title,
            string installmentNumber,
            string installmentCost,
            InvoiceItemType type,
            string purchaseDay)
        {
            Id = id;
            InstallmentNumber = installmentNumber;
            InstallmentCost = installmentCost;
            Type = type;
            PurchaseDay = purchaseDay;
            Title = title;
        }

        public String Id { get; set; }

        public String InstallmentNumber { get; set; }

        public String InstallmentCost { get; set; }

        public InvoiceItemType Type { get; set; }

        public String PurchaseDay { get; set; }

        public String Title { get; set; }
    }
}

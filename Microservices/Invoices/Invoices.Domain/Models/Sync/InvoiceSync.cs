using Invoices.Domain.Enums;
using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceSync
    {
        public InvoiceSync(string id,
            string totalCost,
            string closeDate,
            string dueDate,
            string paymentDate,
            PaymentStatus paymentStatus,
            List<InvoiceItemSync> items)
        {
            Id = id;
            TotalCost = totalCost;
            CloseDate = closeDate;
            Items = items;
            DueDate = dueDate;
            PaymentDate = paymentDate;
            PaymentStatus = paymentStatus;
        }

        public string Id { get; set; }

        public string TotalCost { get; set; }

        public string CloseDate { get; set; }

        public List<InvoiceItemSync> Items { get; set; }

        public string DueDate { get; set; }

        public string PaymentDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}

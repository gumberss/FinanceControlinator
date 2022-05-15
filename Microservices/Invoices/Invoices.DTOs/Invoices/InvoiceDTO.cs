using Invoices.DTOs.Invoices.Enum;
using System;
using System.Collections.Generic;

namespace Invoices.DTOs.Invoices
{
    public record InvoiceDTO
    {
        public decimal TotalCost { get; set; }

        public DateTime CloseDate { get; set; }

        public List<InvoiceItemDTO> Items { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}

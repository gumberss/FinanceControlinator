using System;
using System.Collections.Generic;

namespace FinanceControlinator.Events.Invoices.DTOs
{
    public class InvoiceDTO
    {
        public String Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public decimal TotalCost { get; set; }

        public List<InvoiceItemDTO> Items { get; set; }

        public DateTime DueDate { get; set; }
    }
}

﻿using System;

namespace FinanceControlinator.Events.Invoices.DTOs
{
    public class InvoiceItemDTO
    {
        public String Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public String ExpenseId { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal InstallmentCost { get; set; }

        public int Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public String Location { get; set; }

        public String Title { get; set; }
    }
}

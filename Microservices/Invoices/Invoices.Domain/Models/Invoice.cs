using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Models
{
    public class Invoice : Entity<String>
    {
        public Invoice(DateTime dueDate)
        {
            DueDate = dueDate;
            Items = new List<InvoiceItem>();
            CreatedDate = DateTime.Now;
        }

        public decimal TotalCost { get => Items.Sum(x => x.InstallmentCost); }

        public List<InvoiceItem> Items { get; set; }

        public DateTime CloseDate { get; set; }

        public DateTime DueDate { get; set; }

        public Invoice AddNew(InvoiceItem invoiceItem)
        {
            Items.Add(invoiceItem);

            return this;
        }
    }
}

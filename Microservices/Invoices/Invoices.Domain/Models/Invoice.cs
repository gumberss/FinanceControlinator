using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Models
{
    public class Invoice : Entity<String>
    {
        protected Invoice() { }

        public Invoice(DateTime dueDate)
        {
            Id = Guid.NewGuid().ToString();
            DueDate = dueDate;
            Items = new List<InvoiceItem>();
            CreatedDate = DateTime.Now;
        }

        public decimal TotalCost { get => Items.Sum(x => x.InstallmentCost); }

        public List<InvoiceItem> Items { get; set; }

        public DateTime DueDate { get; set; }

        public Invoice AddNew(InvoiceItem invoiceItem)
        {
            Items.Add(invoiceItem);

            return this;
        }

        public Invoice WasUpdated()
        {
            UpdatedDate = DateTime.Now;

            return this;
        }
    }
}

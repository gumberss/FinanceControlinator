using FinanceControlinator.Common.Entities;
using Invoices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Models
{
    public class Invoice : Entity<String>
    {
        protected Invoice() { }

        public Invoice(DateTime closeDate)
        {
            Id = Guid.NewGuid().ToString();
            CloseDate = closeDate;
            Items = new List<InvoiceItem>();
            CreatedDate = DateTime.Now;
            PaymentStatus = PaymentStatus.Opened;

            DueDate = closeDate.AddDays(7);// todo: Days to pay the invoice: Invoice Configuration
        }

        public decimal TotalCost { get => Items.Sum(x => x.InstallmentCost); }
        
        public DateTime CloseDate { get; private set; }

        public List<InvoiceItem> Items { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime? PaymentDate { get; private set; }

        public PaymentStatus PaymentStatus { get; private set; }

        public String Title => DueDate.ToString();

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

        public Invoice WasPaidIn(DateTime date)
        {
            PaymentDate = date;
            PaymentStatus = PaymentStatus.Paid;
            WasUpdated();

            return this;
        }
    }
}

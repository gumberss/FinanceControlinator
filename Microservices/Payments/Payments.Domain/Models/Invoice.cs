using FinanceControlinator.Common.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Payments.Domain.Models
{
    public class Invoice : Entity<String>
    {
        protected Invoice() { }

        public decimal TotalCost { get; set; }

        public List<InvoiceItem> Items { get; set; }

        public DateTime DueDate { get; set; }

        public InvoicePaymentStatus PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public Invoice ReplaceItems(List<InvoiceItem> items)
        {
            Items = new List<InvoiceItem>(items);

            return this;
        }

        public Invoice UpdateFrom(Invoice changedInvoice)
        {
            TotalCost = changedInvoice.TotalCost;
            DueDate = changedInvoice.DueDate;
            PaymentStatus = changedInvoice.PaymentStatus;
            PaymentDate = changedInvoice.PaymentDate;
            UpdatedDate = changedInvoice.UpdatedDate;

            return this;
        }
    }
}

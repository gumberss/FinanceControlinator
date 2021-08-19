using FinanceControlinator.Common.Entities;
using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounts.Domain.Models
{
    public class Invoice : Entity<String>
    {
        protected Invoice() { }

        public decimal TotalCost { get; set; }

        public List<InvoiceItem> Items { get; set; }

        public DateTime DueDate { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime AccountDate { get; set; }

        public Invoice ReplaceItems(List<InvoiceItem> items)
        {
            Items = new List<InvoiceItem>(items);

            return this;
        }

        public Invoice UpdateFrom(Invoice changedInvoice)
        {
            TotalCost = changedInvoice.TotalCost;
            DueDate = changedInvoice.DueDate;
            AccountStatus = changedInvoice.AccountStatus;
            AccountDate = changedInvoice.AccountDate;
            UpdatedDate = changedInvoice.UpdatedDate;

            return this;
        }
    }
}

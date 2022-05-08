using FinanceControlinator.Common.Entities;
using Invoices.Domain.Enums;
using System;

namespace Invoices.Domain.Models
{
    public class InvoiceItem : Entity<String>
    {
        protected InvoiceItem() { }

        public InvoiceItem(int installmentNumber, decimal installmentCost)
        {
            Id = Guid.NewGuid().ToString();
            InstallmentNumber = installmentNumber;
            InstallmentCost = installmentCost;
            CreatedDate = DateTime.Now;
        }

        public String ExpenseId { get; private set; }

        public int InstallmentNumber { get; private set; }

        public decimal InstallmentCost { get; private set; }

        public InvoiceItemType Type { get; private set; }

        public DateTime PurchaseDay { get; private set; }

        public String Location { get; private set; }

        public String Title { get; private set; }

        public String DetailsPath { get; private set; }

        public InvoiceItem From(Expense expense)
        {
            ExpenseId = expense.Id;
            Type = expense.Type;
            PurchaseDay = expense.PurchaseDay;
            Location = expense.Location;
            Title = expense.Title;
            DetailsPath = expense.DetailsPath;

            return this;
        }

        public InvoiceItem WithType(InvoiceItemType type)
        {
            Type = type;

            return this;
        }
    }
}

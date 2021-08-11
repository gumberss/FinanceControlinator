using FinanceControlinator.Common.Entities;
using Invoices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public String ExpenseId { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal InstallmentCost { get; set; }

        public InvoiceItemType Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public String Location { get; set; }

        public String Title { get; set; }

        public InvoiceItem From(Expense expense)
        {
            ExpenseId = expense.Id;
            Type = (InvoiceItemType)expense.Type;
            PurchaseDay = expense.PurchaseDay;
            Location = expense.Location;
            Title = expense.Title;

            return this;
        }
    }
}

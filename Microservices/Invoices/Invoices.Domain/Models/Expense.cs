using Invoices.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invoices.Domain.Models
{
    public class Expense : Entity<String>
    {
        public String Title { get; set; }

        public DateTime PurchaseDay { get; set; }

        public InvoiceItemType Type { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public decimal TotalCost { get; set; }

        public String DetailsPath { get; set; }
    }
}

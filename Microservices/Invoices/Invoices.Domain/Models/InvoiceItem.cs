using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Domain.Models
{
    public class InvoiceItem : Entity<String>
    {
        public String ExpenseId { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal InstallmentCost { get; set; }

        //type?? Investment, Helth...
    }
}

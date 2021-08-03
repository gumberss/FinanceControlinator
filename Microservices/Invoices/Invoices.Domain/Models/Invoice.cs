using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Domain.Models
{
    public class Invoice : Entity<String>
    {
        public DateTime Month { get; set; }

        public decimal TotalCost { get; set; }

        public IEnumerable<InvoiceItem> ExpenseId { get; set; }




    }
}

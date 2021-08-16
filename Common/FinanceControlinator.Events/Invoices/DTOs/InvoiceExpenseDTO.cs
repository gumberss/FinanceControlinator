using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlinator.Events.Invoices.DTOs
{
    public class InvoiceExpenseDTO
    {
        public Guid Id { get; set; }

        public String Title { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public int InstallmentsCount { get; set; }

        public decimal TotalCost { get; set; }

        public String Location { get; set; }

        public DateTime CreatedDate { get; set; }

        public String DetailsPath { get; set; } // make front-end able to redirect to details of this expense 
    }
}

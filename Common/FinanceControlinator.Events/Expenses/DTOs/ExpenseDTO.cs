using System;
using System.Collections.Generic;

namespace FinanceControlinator.Events.Expenses.DTOs
{
    public class ExpenseDTO
    {
        public Guid Id { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItemDTO> Items { get; set; }
    }
}

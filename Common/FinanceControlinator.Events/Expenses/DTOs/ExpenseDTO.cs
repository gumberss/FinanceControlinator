using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceControlinator.Events.Expenses.DTOs
{
    public class ExpenseDTO
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        public bool IsRecurrent { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItemDTO> Items { get; set; }
    }
}

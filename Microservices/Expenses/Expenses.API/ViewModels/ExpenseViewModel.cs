using Expenses.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModels
{
    public class ExpenseViewModel
    {
        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public ExpenseType Type { get; init; }

        public bool IsRecurrent { get; init; }

        public String Location { get; init; }

        public String Observation { get; init; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItemViewModel> Items { get; init; }
    }
}

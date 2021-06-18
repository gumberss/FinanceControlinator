using Expenses.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModels
{
    public class ExpenseItemViewModel
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid ExpenseId { get; set; }

    }
}

using System;

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

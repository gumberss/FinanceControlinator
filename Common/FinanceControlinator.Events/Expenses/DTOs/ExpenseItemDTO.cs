using System;

namespace FinanceControlinator.Events.Expenses.DTOs
{
    public class ExpenseItemDTO
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public decimal Cost { get; set; }

        public int Amount { get; set; }

        public Guid ExpenseId { get; set; }
    }
}

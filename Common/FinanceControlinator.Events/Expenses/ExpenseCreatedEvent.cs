using FinanceControlinator.Events.Expenses.DTOs;

namespace FinanceControlinator.Events.Expenses
{
    public class ExpenseCreatedEvent
    {
        public ExpenseDTO Expense { get; set; }
    }
}

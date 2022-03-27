using System;

namespace Expenses.Domain.Models.Expenses
{
    public class ExpenseOverview
    {
        public ExpenseOverview(String text)
        {
            Text = text;
        }

        public String Text { get; set; }
    }
}

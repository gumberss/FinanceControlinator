using System;

namespace Expenses.Domain.Models.Expenses.Overviews
{
    public class ExpenseBrief
    {
        public ExpenseBrief(String text)
        {
            Text = text;
        }

        public String Text { get; set; }
    }
}

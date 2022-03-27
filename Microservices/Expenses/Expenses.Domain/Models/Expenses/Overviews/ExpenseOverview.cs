using System;
using System.Collections.Generic;

namespace Expenses.Domain.Models.Expenses.Overviews
{
    public class ExpenseOverview
    {
        public List<ExpenseBrief> Briefs { get; private set; } 
        public List<ExpensePartition> ExpensePartitions { get; private set; }

        public ExpenseOverview(List<ExpenseBrief> briefs, List<ExpensePartition> expensePartitions)
        {
            Briefs = briefs;
            ExpensePartitions = expensePartitions;
        }
    }
}

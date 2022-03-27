using System;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewDTO
    {
        public List<ExpenseBriefDTO> Briefs { get; set; }
        public List<ExpensePartitionDTO> ExpensePartitions { get; set; }

    }

    public class ExpenseBriefDTO
    {
        public String Text { get; set; }
    }

    public class ExpensePartitionDTO
    {
        public String Type { get; set; }

        public float Percent { get; set; }

        public decimal TotalValue { get; set; }
    }
}

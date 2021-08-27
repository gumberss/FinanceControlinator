using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Expenses.Domain.Models.Expenses
{
    public class ExpenseItem : Entity<Guid>
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public decimal Cost { get; set; }

        public int Amount { get; set; }

        public Guid ExpenseId { get; set; }

        public Expense Expense { get; set; }

        internal ExpenseItem UpdateFrom(ExpenseItem itemFounded)
        {
            Cost = itemFounded.Cost;
            Amount = itemFounded.Amount;
            Name = itemFounded.Name;
            Description = itemFounded.Description;

            return this;
        }
    }

    public class ExpenseItemComparer : IEqualityComparer<ExpenseItem>
    {
        public bool Equals(ExpenseItem x, ExpenseItem y) => x.Id == y.Id;

        public int GetHashCode([DisallowNull] ExpenseItem obj) => obj.GetHashCode();
    }
}

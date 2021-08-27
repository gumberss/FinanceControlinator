using Expenses.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Models.Expenses
{
    public class Expense : Entity<Guid>
    {
        public String Title { get; private set; }

        public String Description { get; private set; }

        public DateTime PurchaseDay { get; private set; }

        public ExpenseType Type { get; private set; }

        public int InstallmentsCount { get; private set; }

        public String Location { get; private set; }

        public String Observation { get; private set; }

        public decimal TotalCost { get; private set; }

        public List<ExpenseItem> Items { get; private set; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);

        public Expense ChangeTotalCost(decimal totalCost)
        {
            TotalCost = totalCost;

            return this;
        }

        public Expense ChangeTotalCost(int isntallmentsCoint)
        {
            InstallmentsCount = isntallmentsCoint;

            return this;
        }

        public Expense UpdateItems(List<ExpenseItem> updatedItems)
        {
            foreach (var item in updatedItems)
            {
                var expenseItem = Items.Find(x => x.Id == item.Id);

                if (expenseItem is null) continue;

                expenseItem.UpdateFrom(item);
            }
            
            return this;
        }

        public Expense RemoveItems(List<ExpenseItem> itemsToRemove)
        {
            Items = Items.Except(itemsToRemove).ToList();

            return this;
        }

        public Expense AddItems(List<ExpenseItem> updatedItems)
        {
            Items.AddRange(updatedItems);

            return this;
        }
    }
}

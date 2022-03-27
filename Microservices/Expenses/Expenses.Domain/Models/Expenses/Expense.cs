using Expenses.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Models.Expenses
{
    public class Expense : Entity<Guid>
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime PurchaseDate { get; set; }

        public ExpenseType Type { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItem> Items { get; set; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);

        public Expense ChangeTotalCost(decimal totalCost)
        {
            TotalCost = totalCost;

            return this;
        }

        public Expense ChangeTotalCost(int installmentsCount)
        {
            InstallmentsCount = installmentsCount;

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

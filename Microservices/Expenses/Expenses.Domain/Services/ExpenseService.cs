using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Services
{
    public class ExpenseService : IExpenseService
    {
        public (List<ExpenseItem> toDelete, List<ExpenseItem> toAdd, List<ExpenseItem> toUpdate) SegregateItems(Expense expense, Expense registeredExpense)
        {
            var expenseItemComparer = new ExpenseItemComparer();

            var toDelete = registeredExpense.Items.Except(expense.Items, expenseItemComparer).ToList();
            var toAdd = expense.Items.Except(registeredExpense.Items, expenseItemComparer).ToList();
            var toUpdate = registeredExpense.Items.Intersect(expense.Items, expenseItemComparer).ToList();

            return (toAdd, toUpdate, toDelete);
        }

        public bool TotalCostIsValid(Expense expense, List<Invoice> invoicesWithExpenseCosts)
        {
            var totalExpensePaid = invoicesWithExpenseCosts
                            .SelectMany(inv => inv.ItemsFrom(expense))
                            .Sum(item => item.InstallmentCost);

            return expense.TotalCost >= totalExpensePaid;
        }

        public bool InstallmentsCountIsValid(Expense expense, List<Invoice> invoicesWithExpenseCosts)
        {
            return expense.InstallmentsCount >= invoicesWithExpenseCosts.Count;
        }
    }
}

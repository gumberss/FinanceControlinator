using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using System.Collections.Generic;

namespace Expenses.Domain.Interfaces.Services
{
    public interface IExpenseService
    {
        (List<ExpenseItem> toDelete, List<ExpenseItem> toAdd, List<ExpenseItem> toUpdate) SegregateItems(Expense expense, Expense registeredExpense);

        bool TotalCostIsValid(Expense expense, List<Invoice> invoicesWithExpenseCosts);

        bool InstallmentsCountIsValid(Expense expense, List<Invoice> invoicesWithExpenseCosts);
    }
}

using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Repositories;
using System;

namespace Expenses.Data.Repositories
{
    public interface IExpenseRepository : IRepository<Expense, Guid>
    {

    }

    public class ExpenseRepository : Repository<Expense, ExpenseDbContext, Guid>, IExpenseRepository
    {
        public ExpenseRepository(IExpenseDbContext context) : base(context as ExpenseDbContext)
        {

        }
    }
}

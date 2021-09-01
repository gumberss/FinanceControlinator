using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Repositories;
using System;

namespace Expenses.Data.Repositories
{
    public interface IExpenseItemRepository : IRepository<ExpenseItem, Guid>
    {

    }

    public class ExpenseItemRepository : Repository<ExpenseItem, ExpenseDbContext, Guid>, IExpenseItemRepository
    {
        public ExpenseItemRepository(IExpenseDbContext context) : base(context as ExpenseDbContext)
        {

        }
    }
}

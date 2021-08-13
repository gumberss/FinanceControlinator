using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Repositories;

namespace Expenses.Data.Repositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {

    }

    public class ExpenseRepository : Repository<Expense, ExpenseDbContext>, IExpenseRepository
    {
        public ExpenseRepository(ExpenseDbContext context) : base(context)
        {
            
        }
    }
}

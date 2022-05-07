using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Repositories;
using FinanceControlinator.Common.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Data.Repositories
{
    public interface IExpenseRepository : IRepository<Expense, Guid>
    {
        Task<Result<List<Expense>, BusinessException>> GetPaginationAsync(int page, int count, Guid userId);
    }

    public class ExpenseRepository : Repository<Expense, ExpenseDbContext, Guid>, IExpenseRepository
    {
        public ExpenseRepository(IExpenseDbContext context) : base(context as ExpenseDbContext)
        {

        }

        public async Task<Result<List<Expense>, BusinessException>> GetPaginationAsync(int page, int count, Guid userId)
        {
            return await Result.Try(GetQueryable()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.PurchaseDate)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync());
        }
    }
}

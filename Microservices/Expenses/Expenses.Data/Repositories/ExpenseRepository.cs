using Expenses.Data.Contexts;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Repositories;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Repositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {

    }

    public class ExpenseRepository : IExpenseRepository
    {
        public ExpenseRepository(ExpenseDbContext context)
        {

        }

        public Task<Result<Expense, BusinessException>> Add(Expense entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> Delete(Expense entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> Delete(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Expense, BusinessException>> Get(params Expression<Func<Expense, bool>>[] where)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Expense>, BusinessException>> GetAll(params Expression<Func<Expense, bool>>[] where)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Expense, BusinessException>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Expense, BusinessException>> Update(Expense entity)
        {
            throw new NotImplementedException();
        }
    }
}

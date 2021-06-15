using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Repositories;
using FinanceControlinator.Common.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Commons
{
    public class Repository<T> :   IRepository<T> where T : Entity
    {
        public Repository(DbContext context) 
        {

        }

        public Task<Result<T, BusinessException>> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> Delete(T entity)
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

        public Task<Result<T, BusinessException>> Get(params Expression<Func<T, bool>>[] where)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<T>, BusinessException>> GetAll(params Expression<Func<T, bool>>[] where)
        {
            throw new NotImplementedException();
        }

        public Task<Result<T, BusinessException>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<T, BusinessException>> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}

using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Repositories
{
    public interface IRepository<T, TId> : IRepositoryBase where T : IEntity<TId>
    {
        Task<Result<List<T>, BusinessException>> GetAllAsync(Expression<Func<T, object>> include = null, params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> GetAsync(params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> GetByIdAsync(TId id);

        public Task<Result<T, BusinessException>> AddAsync(T entity);

        public Task<Result<T, BusinessException>> UpdateAsync(T entity);

        public Task<Result<bool, BusinessException>> DeleteAsync(T entity);

        public Task<Result<bool, BusinessException>> DeleteAsync(TId id);

        public Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<TId> ids);
    }
}

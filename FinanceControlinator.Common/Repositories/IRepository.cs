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
    public interface IRepository<T> : IRepositoryBase where T : IEntity
    {
        public Task<Result<List<T>, BusinessException>> GetAll(params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> Get(params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> GetById(Guid id);

        public Task<Result<T, BusinessException>> Add(T entity);

        public Task<Result<T, BusinessException>> Update(T entity);

        public Task<Result<bool, BusinessException>> Delete(T entity);

        public Task<Result<bool, BusinessException>> Delete(Guid id);

        public Task<Result<bool, BusinessException>> Delete(IEnumerable<Guid> ids);
    }
}

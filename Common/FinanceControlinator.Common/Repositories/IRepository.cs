﻿using CleanHandling;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Repositories
{
    public interface IRepository<T, TId> : IRepositoryBase where T : IEntity<TId>
    {
        Task<Result<List<T>, BusinessException>> GetAllAsync(Expression<Func<T, object>> include = null, params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> GetAsync(params Expression<Func<T, bool>>[] where);

        public Task<Result<T, BusinessException>> GetByIdAsync(TId id, Expression<Func<T, object>> include = null);

        public Task<Result<T, BusinessException>> AddAsync(T entity);

        public Task<Result<IEnumerable<T>, BusinessException>> AddAsync(IEnumerable<T> entities);

        public Task<Result<T, BusinessException>> UpdateAsync(T entity);

        public Task<Result<bool, BusinessException>> DeleteAsync(T entity);

        public Task<Result<bool, BusinessException>> DeleteAsync(TId id);

        public Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<TId> ids);

        public Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<T> entities);
    }
}

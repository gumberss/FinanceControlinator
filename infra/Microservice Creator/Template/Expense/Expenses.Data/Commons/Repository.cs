﻿using FinanceControlinator.Common.Entities;
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
    public class Repository<TEntity, TContext> : IRepository<TEntity>
           where TEntity : class, IEntity
           where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(TContext context)
        {
            _context = context;

            _dbSet = _context.Set<TEntity>();
        }

        public async Task<Result<TEntity, BusinessException>> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }

            return entity;
        }

        public Task<Result<bool, BusinessException>> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<TEntity>, BusinessException>> GetAllAsync(Expression<Func<TEntity, object>> include = null, params Expression<Func<TEntity, bool>>[] where)
        {
            try
            {
                IQueryable<TEntity> dbSet = _dbSet;

                if (where is not null)
                {
                    foreach (var item in where)
                    {
                        dbSet = dbSet.Where(item);
                    }
                }

                if (include is not null)
                    dbSet = dbSet.Include(include);

                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }
        public async Task<Result<List<TEntity>, BusinessException>> GetAllIncludeAsync(params Expression<Func<TEntity, bool>>[] where)
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }


        public Task<Result<TEntity, BusinessException>> GetAsync(params Expression<Func<TEntity, bool>>[] where)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TEntity, BusinessException>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TEntity, BusinessException>> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

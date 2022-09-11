using CleanHandling;
using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Repositories;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Invoices.Data.Commons
{
    public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
           where TEntity : class, IEntity<TEntityId>
    {
        private IAsyncDocumentSession _session;

        public Repository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<Result<TEntity, BusinessException>> AddAsync(TEntity entity)
        {
            var result = await Result.Try(_session.StoreAsync(entity));

            return result.IsFailure
                ? result.Error
                : entity;
        }

        public Task<Result<IEnumerable<TEntity>, BusinessException>> AddAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(TEntity entity)
        {
            try
            {
                _session.Delete(entity);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(Guid id)
        {
            try
            {
                _session.Delete(id.ToString());

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<TEntityId> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    _session.Delete(id.ToString());
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(TEntityId id)
        {
            return await DeleteAsync(new List<TEntityId> { id });
        }

        public Task<Result<bool, BusinessException>> DeleteAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<TEntity>, BusinessException>> GetAllAsync(Expression<Func<TEntity, object>> include = null, params Expression<Func<TEntity, bool>>[] where)
        {
            try
            {
                var query = _session.Query<TEntity>();

                if (include is not null)
                    query = query.Include(include);

                IQueryable<TEntity> queryableQuery = query;

                if (where is not null)
                {
                    foreach (var item in where)
                    {
                        queryableQuery = queryableQuery.Where(item);
                    }
                }

                return await queryableQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<TEntity, BusinessException>> GetAsync(params Expression<Func<TEntity, bool>>[] where)
        {
            try
            {
                IQueryable<TEntity> query = _session.Query<TEntity>();

                foreach (var item in where)
                {
                    query = query.Where(item);
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }
        public async Task<Result<TEntity, BusinessException>> GetByIdAsync(TEntityId id, Expression<Func<TEntity, object>> include = null)
        {
            var query = _session.Query<TEntity>();

            if (include is not null)
                query = query.Include(include);

            IQueryable<TEntity> queryableQuery = query;

            var result = await Result.Try(queryableQuery.FirstOrDefaultAsync<TEntity>(x => x.Id.ToString() == id.ToString()));

            if (result.IsFailure)
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, result.Error);

            return result.Value;
        }

        public Task<Result<TEntity, BusinessException>> UpdateAsync(TEntity entity)
        {
            Result<TEntity, BusinessException> result = entity;

            return Task.FromResult(result);
        }
    }
}

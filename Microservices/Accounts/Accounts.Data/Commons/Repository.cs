using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Repositories;
using FinanceControlinator.Common.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Accounts.Data.Commons
{
    public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
           where TEntity : class, IEntity<TEntityId>
    {
        private Container _container;

        public Repository(
          CosmosClient cosmosDbClient,
          string databaseName,
          string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task<Result<TEntity, BusinessException>> AddAsync(TEntity entity)
        {
            var result = await Result.Try(_container.CreateItemAsync(entity, new PartitionKey(entity.Id.ToString())));

            return result.IsFailure
                ? result.Error
                : entity;
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(TEntity entity)
        {
            try
            {
                var result = await Result.Try(_container.DeleteItemAsync<TEntity>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString())));

                return result.IsFailure
                    ? result.Error
                    : true;
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<bool, BusinessException>> DeleteAsync(TEntityId id)
        {
            try
            {
                var result = await Result.Try(_container.DeleteItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString())));

                return result.IsFailure
                    ? result.Error
                    : true;
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
                    await _container.DeleteItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
                }

                return true;
            }
            catch (Exception ex)
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task<Result<List<TEntity>, BusinessException>> GetAllAsync(Expression<Func<TEntity, object>> include = null, params Expression<Func<TEntity, bool>>[] where)
        {
            var query = _container.GetItemLinqQueryable<TEntity>();

            if (where is not null)
                foreach (var item in where) query = (IOrderedQueryable<TEntity>)query.Where(item);

            var iterator = query.ToFeedIterator<TEntity>();

            var results = new List<TEntity>();

            using (iterator)
            {
                while (iterator.HasMoreResults)
                {
                    foreach (var item in await iterator.ReadNextAsync())
                    {
                        results.Add(item);
                    }
                }
            }

            return results;
        }

        public async Task<Result<TEntity, BusinessException>> GetAsync(params Expression<Func<TEntity, bool>>[] where)
        {
            var query = _container.GetItemLinqQueryable<TEntity>();

            if (where is not null)
                foreach (var item in where) query.Where(item);

            var iterator = query.ToFeedIterator<TEntity>();

            TEntity result = default;

            using (iterator)
            {
                if (iterator.HasMoreResults)
                {
                    result = (await iterator.ReadNextAsync()).FirstOrDefault();
                }
            }

            return result;
        }

        public async Task<Result<TEntity, BusinessException>> GetByIdAsync(TEntityId id, Expression<Func<TEntity, object>> include = null)
        {
            try
            {

                var response = await _container.ReadItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return default(TEntity);
            }
        }

        public async Task<Result<TEntity, BusinessException>> GetByIdAsync(TEntityId id)
        {
            try
            {
                var response = await _container.ReadItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return default(TEntity);
            }
        }

        public async Task<Result<TEntity, BusinessException>> UpdateAsync(TEntity entity)
        {
            try
            {
                var result = await _container.UpsertItemAsync(entity, new PartitionKey(entity.Id.ToString()));

                return result.Resource;
            }
            catch (Exception ex) 
            {
                return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}

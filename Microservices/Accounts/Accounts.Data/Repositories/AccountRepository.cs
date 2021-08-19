using Accounts.Data.Commons;
using Accounts.Domain.Models;
using FinanceControlinator.Common.Repositories;
using Microsoft.Azure.Cosmos;
using System;

namespace Accounts.Data.Repositories
{
    public interface IAccountRepository : IRepository<Account, String>
    {

    }

    public class AccountRepository : Repository<Account, String>, IAccountRepository
    {
        public AccountRepository(CosmosClient cosmosDbClient) : base(cosmosDbClient, "Accounts", "AccountsContainer")
        {

        }
    }
}

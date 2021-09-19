using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Repositories;
using System;

namespace PiggyBanks.Data.Repositories
{
    public interface IPiggyBankRepository : IRepository<PiggyBank, Guid>
    {

    }

    public class PiggyBankRepository : Repository<PiggyBank, PiggyBankDbContext, Guid>, IPiggyBankRepository
    {
        public PiggyBankRepository(PiggyBankDbContext context) : base(context)
        {
            
        }
    }
}

using FinanceControlinator.Common.Repositories;
using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using System;

namespace PiggyBanks.Data.Repositories
{
    public interface ITransferRepository : IRepository<Transfer, Guid>
    {
    }

    public class TransferRepository : Repository<Transfer, PiggyBankDbContext, Guid>, ITransferRepository
    {
        public TransferRepository(IPiggyBankDbContext context) : base(context as PiggyBankDbContext)
        {

        }
    }
}

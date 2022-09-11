using CleanHandling;
using FinanceControlinator.Common.Repositories;
using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PiggyBanks.Data.Repositories
{
    public interface IPiggyBankRepository : IRepository<PiggyBank, Guid>
    {
        Task<Result<bool, BusinessException>> ExistsPiggyBankByTitle(String title);
    }

    public class PiggyBankRepository : Repository<PiggyBank, PiggyBankDbContext, Guid>, IPiggyBankRepository
    {
        public PiggyBankRepository(IPiggyBankDbContext context) : base(context as PiggyBankDbContext)
        {

        }

        public async Task<Result<bool, BusinessException>> ExistsPiggyBankByTitle(String title)
        {
            return await Result.Try(() =>
                _dbSet
                    .Where(x => x.Title == title)
                    .Any()
           );
        }
    }
}

using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Repositories;
using System;
using System.Linq;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Common.Exceptions;
using System.Threading.Tasks;
using PiggyBanks.Data.Interfaces.Contexts;

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

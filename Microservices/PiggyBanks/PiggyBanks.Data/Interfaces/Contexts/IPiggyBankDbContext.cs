using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace PiggyBanks.Data.Interfaces.Contexts
{
    public interface IPiggyBankDbContext
    {
        Task<Result<int, BusinessException>> Commit();
    }
}

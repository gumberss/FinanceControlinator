using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiggyBanks.Application.Interfaces.AppServices
{
    public interface IPiggyBankAppService
    {
        public Task<Result<PiggyBank, BusinessException>> RegisterPiggyBank(PiggyBank piggyBank);

        Task<Result<List<PiggyBank>, BusinessException>> GetAllPiggyBanks();

        Task<Result<List<PiggyBank>, BusinessException>> GetMonthPiggyBanks();

        Task<Result<List<PiggyBank>, BusinessException>> GetLastMonthPiggyBanks();
    }
}

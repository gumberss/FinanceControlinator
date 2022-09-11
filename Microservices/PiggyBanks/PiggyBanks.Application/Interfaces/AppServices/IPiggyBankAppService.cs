using CleanHandling;
using PiggyBanks.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiggyBanks.Application.Interfaces.AppServices
{
    public interface IPiggyBankAppService
    {
        public Task<Result<PiggyBank, BusinessException>> RegisterPiggyBank(PiggyBank piggyBank);

        Task<Result<List<PiggyBank>, BusinessException>> GetAllPiggyBanks();
        Task<Result<PiggyBank, BusinessException>> Save(decimal value);
        Task<Result<List<PiggyBank>, BusinessException>> RegisterPayment(Invoice invoice);
    }
}

using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using PiggyBanks.Domain.Models;
using System.Threading.Tasks;

namespace PiggyBanks.Application.Interfaces.AppServices
{
    public interface ITransferAppService
    {
        Task<Result<Transfer, BusinessException>> RegisterTransfer(Transfer transfer);
    }
}

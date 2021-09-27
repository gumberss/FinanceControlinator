using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Application.Interfaces.AppServices
{
    public interface IAccountAppService
    {
        Task<Result<List<AccountChange>, BusinessException>> Register(Payment paymentRequested);
        Task<Result<Account, BusinessException>> Withdraw(Guid? accountId, decimal amount);
    }
}

using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Microsoft.Extensions.Logging;
using Accounts.Domain.Localizations;
using Accounts.Domain.Models;
using System.Linq;
using Accounts.Domain.Interfaces.Services;
using System.Collections.Generic;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace Accounts.Application.AppServices
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IAccountAppService> _logger;
        private readonly IAccountService _accountService;

        public AccountAppService(
            IAccountRepository accountRepository, 
            ILocalization localization, 
            ILogger<IAccountAppService> logger, 
            IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _localization = localization;
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<Result<List<AccountChange>, BusinessException>> Register(Payment paymentRequested)
        {
            var sourceIds = paymentRequested.SourceIds();

            var accounts = await _accountRepository
                .GetAllAsync(where: acc => sourceIds.Contains(acc.Id));

            if (accounts.IsFailure) return accounts.Error;

            var accountsAreAbleToPay = _accountService.AbleToPay(paymentRequested, accounts);

            if (accountsAreAbleToPay)
            {
                var changes = _accountService.Pay(paymentRequested, accounts);

                return changes;
            }

            return new BusinessException(System.Net.HttpStatusCode.BadRequest
                , new ErrorData(_localization.SOME_ACCOUNT_IS_NOT_ABLE_TO_PAY_THE_REQUESTED_AMOUNT));
        }
    }
}

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
using System;
using System.Net;

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

                foreach (var account in accounts.Value)
                    await _accountRepository.UpdateAsync(account);

                return changes;
            }

            return new BusinessException(System.Net.HttpStatusCode.BadRequest
                , new ErrorData(_localization.SOME_ACCOUNT_IS_NOT_ABLE_TO_PAY_THE_REQUESTED_AMOUNT));
        }

        public async Task<Result<Account, BusinessException>> Withdraw(Guid? accountId, decimal amount)
        {
            if (amount <= 0)
                return new BusinessException(HttpStatusCode.BadRequest, "");

            if (!accountId.HasValue)
                return new BusinessException(HttpStatusCode.BadRequest, "");

            var account = await _accountRepository.GetByIdAsync(accountId.Value.ToString());

            if (account.IsFailure) return account.Error;

            if (!account.Value.AbleToPay(amount))
                return new BusinessException(HttpStatusCode.BadRequest, "");

            account.Value.Withdraw(amount);

            return account;
        }
    }
}

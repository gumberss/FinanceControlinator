using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Microsoft.Extensions.Logging;
using Accounts.Domain.Localizations;

namespace Accounts.Application.AppServices
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILocalization _localization;
        private readonly ILogger<IAccountAppService> _logger;

        public AccountAppService(
                IAccountRepository accountRepository
                , ILocalization localization
                , ILogger<IAccountAppService> logger
            )
        {
            _accountRepository = accountRepository;
            _localization = localization;
            _logger = logger;

        }
    }
}

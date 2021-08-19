using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Accounts.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;
using Accounts.Domain.Localizations;

namespace Accounts.Application.AppServices
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountItemValidator _accountItemValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IAccountAppService> _logger;

        public AccountAppService(
                IAccountRepository accountRepository
                , IAccountItemValidator accountItemValidator
                , ILocalization localization
                , ILogger<IAccountAppService> logger
            )
        {
            _accountRepository = accountRepository;
            _accountItemValidator = accountItemValidator;
            _localization = localization;
            _logger = logger;

        }
    }
}

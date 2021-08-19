using AutoMapper;
using Microsoft.Extensions.Logging;
using Accounts.Application.Interfaces.AppServices;
using MediatR;
using Accounts.Handler.Domain.Cqrs.Events.Commands;
using FinanceControlinator.Common.Utils;
using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using Accounts.Handler.Domain.Cqrs.Events.Queries;
using System.Threading.Tasks;
using System.Threading;
using Accounts.Data.Repositories;

namespace Accounts.Handler.Domain.Cqrs.Handlers
{
    public class AccountHandler
         : IRequestHandler<AccountReceiveMoneyCommand, Result<Account, BusinessException>>
          , IRequestHandler<AccountWithdrawMoneyCommand, Result<Account, BusinessException>>
          , IRequestHandler<AccountDataQuery, Result<Account, BusinessException>>
    { 
        private readonly IAccountAppService _accountAppService;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountHandler> _logger;
        private readonly IMapper _mapper;

        public AccountHandler(
            IAccountAppService accountAppService
            , IAccountRepository accountRepository
            , ILogger<AccountHandler> logger
            , IMapper mapper
            )
        {
            _accountAppService = accountAppService;
            _accountRepository = accountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<Account, BusinessException>> Handle(AccountDataQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetAsync();
        }

        public Task<Result<Account, BusinessException>> Handle(AccountWithdrawMoneyCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<Account, BusinessException>> Handle(AccountReceiveMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAsync();

            if (account.IsFailure)
            {
                return account.Error;
            }

            account.Value.TotalAmount += request.Amount;

            var updateResult = await _accountRepository.UpdateAsync(account.Value);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            return updateResult.Value;
        }
    }
}

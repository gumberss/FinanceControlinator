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
using System.Collections.Generic;

namespace Accounts.Handler.Domain.Cqrs.Handlers
{
    public class AccountHandler
         : IRequestHandler<AccountReceiveMoneyCommand, Result<Account, BusinessException>>
          , IRequestHandler<AccountWithdrawMoneyCommand, Result<Account, BusinessException>>
          , IRequestHandler<AccountDataQuery, Result<List<Account>, BusinessException>>
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

        public async Task<Result<List<Account>, BusinessException>> Handle(AccountDataQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetAllAsync();
        }

        public Task<Result<Account, BusinessException>> Handle(AccountWithdrawMoneyCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<Account, BusinessException>> Handle(AccountReceiveMoneyCommand request, CancellationToken cancellationToken)
        {
            var dbAccount = await _accountRepository.GetAsync();

            if (dbAccount.IsFailure)
            {
                return dbAccount.Error;
            }

            Result<Account, BusinessException> processResult;

            if(dbAccount.Value is null)
            {
                var account = new Account().Receive(request.Amount);

                processResult = await _accountRepository.AddAsync(account);
            }
            else
            {
                dbAccount.Value.Receive(request.Amount);

                processResult = await _accountRepository.UpdateAsync(dbAccount.Value);
            }

            return processResult;
        }
    }
}

using CleanHandling;
using MediatR;
using Microsoft.Extensions.Logging;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Domain.Cqrs.Handlers
{
    public class PiggyBankHandler
        : IRequestHandler<GetAllPiggyBanksQuery, Result<List<PiggyBank>, BusinessException>>
        , IRequestHandler<RegisterPiggyBankCommand, Result<PiggyBank, BusinessException>>
        , IRequestHandler<SaveMoneyCommand, Result<PiggyBank, BusinessException>>
        , IRequestHandler<RegisterPiggyBanksPaymentCommand, Result<List<PiggyBank>, BusinessException>>

    {
        private readonly IPiggyBankAppService _piggyBankAppService;
        private readonly IPiggyBankDbContext _piggyBankDbContext;

        public PiggyBankHandler(
            IPiggyBankAppService piggyBankAppService
            , ILogger<PiggyBankHandler> logger
            , IPiggyBankDbContext piggyBankDbContext)
        {
            _piggyBankAppService = piggyBankAppService;
            _piggyBankDbContext = piggyBankDbContext;
        }

        public async Task<Result<PiggyBank, BusinessException>> Handle(RegisterPiggyBankCommand request, CancellationToken cancellationToken)
        {
            var registerResult = await _piggyBankAppService.RegisterPiggyBank(request.PiggyBank);

            if (registerResult.IsFailure) return registerResult.Error;

            var saveResult = await _piggyBankDbContext.Commit();

            if (saveResult.IsFailure) return saveResult.Error;

            return registerResult;
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> Handle(GetAllPiggyBanksQuery request, CancellationToken cancellationToken)
        {
            return await _piggyBankAppService.GetAllPiggyBanks();
        }

        public async Task<Result<PiggyBank, BusinessException>> Handle(SaveMoneyCommand request, CancellationToken cancellationToken)
        {
            var result = await _piggyBankAppService.Save(request.Amount);

            if (result.IsFailure) return result.Error;

            var saveResult = await _piggyBankDbContext.Commit();

            if (saveResult.IsFailure) return saveResult.Error;

            return result;
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> Handle(RegisterPiggyBanksPaymentCommand request, CancellationToken cancellationToken)
        {
            var result = await _piggyBankAppService.RegisterPayment(request.Invoice);

            if (result.IsFailure) return result.Error;

            var saveResult = await _piggyBankDbContext.Commit();

            if (saveResult.IsFailure) return saveResult.Error;

            return result;
        }
    }
}

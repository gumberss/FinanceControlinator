using CleanHandling;
using MediatR;
using Microsoft.Extensions.Logging;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events.Transfers;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Domain.Cqrs.Handlers
{
    public class TransfersHandler
        : IRequestHandler<RegisterTransferCommand, Result<Transfer, BusinessException>>
    {
        private readonly ITransferAppService _transferAppService;
        private readonly IPiggyBankDbContext _piggyBankDbContext;

        public TransfersHandler(
            ITransferAppService transferAppService
            , ILogger<TransfersHandler> logger
            , IPiggyBankDbContext piggyBankDbContext)
        {
            _transferAppService = transferAppService;
            _piggyBankDbContext = piggyBankDbContext;
        }

        public async Task<Result<Transfer, BusinessException>> Handle(RegisterTransferCommand request, CancellationToken cancellationToken)
        {
            var registerResult = await _transferAppService.RegisterTransfer(request.Transfer);

            if (registerResult.IsFailure) return registerResult.Error;

            var saveResult = await _piggyBankDbContext.Commit();

            if (saveResult.IsFailure) return saveResult.Error;

            return registerResult;
        }
    }
}

using AutoMapper;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Domain.Cqrs.Handlers
{
    public class PiggyBankHandler
        : IRequestHandler<RegisterPiggyBankCommand, Result<PiggyBank, BusinessException>>
        , IRequestHandler<GetAllPiggyBanksQuery, Result<List<PiggyBank>, BusinessException>>
    {
        private readonly IPiggyBankAppService _piggyBankAppService;
        private readonly ILogger<PiggyBankHandler> _logger;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public PiggyBankHandler(
            IPiggyBankAppService piggyBankAppService
            , ILogger<PiggyBankHandler> logger
            , IBus bus,
            IMapper mapper)
        {
            _piggyBankAppService = piggyBankAppService;
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Result<PiggyBank, BusinessException>> Handle(RegisterPiggyBankCommand request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(this.GetType().Name))
            {
                var result = await _piggyBankAppService.RegisterPiggyBank(request.PiggyBank);

                return result;
            }
        }

        public async Task<Result<List<PiggyBank>, BusinessException>> Handle(GetAllPiggyBanksQuery request, CancellationToken cancellationToken)
        {
            return await _piggyBankAppService.GetAllPiggyBanks();
        }
    }
}

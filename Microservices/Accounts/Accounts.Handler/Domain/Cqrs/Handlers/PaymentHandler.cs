using Accounts.Application.Interfaces.AppServices;
using Accounts.Domain.Models;
using Accounts.Handler.Domain.Cqrs.Events.Commands.Payments;
using AutoMapper;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Events.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Accounts.Handler.Domain.Cqrs.Handlers
{
    public class PaymentHandler
        : IRequestHandler<PayCommand, Result<List<AccountChange>, BusinessException>>
    {
        private readonly IAccountAppService _accountAppService;
        private readonly IMessageBus _bus;
        private readonly IMapper _mapper;

        public PaymentHandler(
            IAccountAppService accountAppService,
            IMessageBus bus,
            IMapper mapper)
        {
            _accountAppService = accountAppService;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Result<List<AccountChange>, BusinessException>> Handle(PayCommand request, CancellationToken cancellationToken)
        {
            var changes = await _accountAppService.Register(request.Payment);

            if (changes.IsFailure)
            {
                await _bus.Publish(new PaymentRejectedEvent
                {
                    Id = Guid.Parse(request.Payment.Id),
                    Reason = changes.Error.Message
                });

                return changes.Error;
            }

            await _bus.Publish(new PaymentConfirmedEvent { Id = Guid.Parse(request.Payment.Id) });

            return changes;
        }
    }
}

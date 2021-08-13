using AutoMapper;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;

namespace Payments.Handler.Domain.Cqrs.Handlers
{
    public class PaymentHandler
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly ILogger<PaymentHandler> _logger;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public PaymentHandler(
            IPaymentAppService paymentAppService
            , ILogger<PaymentHandler> logger
            , IBus bus,
            IMapper mapper)
        {
            _paymentAppService = paymentAppService;
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
        }

    
    }
}

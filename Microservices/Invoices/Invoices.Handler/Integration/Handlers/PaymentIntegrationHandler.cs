using AutoMapper;
using FinanceControlinator.Events.Payments;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers
{
    public class PaymentIntegrationHandler : IConsumer<PaymentPerformedEvent>
    {
        //private readonly IInvoiceAppService _invoiceAppService;
        private readonly IMapper _mapper;

        public PaymentIntegrationHandler(
          //  IInvoiceAppService invoiceAppService,
            IMapper mapper
            )
        {
          //  _invoiceAppService = invoiceAppService;
            _mapper = mapper;
        }

        public Task Consume(ConsumeContext<PaymentPerformedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Invoices.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceHandler
        : IRequestHandler<GetAllInvoicesQuery, Result<List<Invoice>, BusinessException>>
        , IRequestHandler<GetMonthInvoicesQuery, Result<List<Invoice>, BusinessException>>
        , IRequestHandler<GetLastMonthInvoicesQuery, Result<List<Invoice>, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ILogger<InvoiceHandler> _logger;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService
            , ILogger<InvoiceHandler> logger
            , IBus bus,
            IMapper mapper)
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetAllInvoices();
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetMonthInvoice();
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetLastMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetLastMonthInvoice();
        }
    }
}

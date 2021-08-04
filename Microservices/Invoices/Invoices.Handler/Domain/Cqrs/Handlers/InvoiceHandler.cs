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
        :// IRequestHandler<RegisterInvoiceCommand, Result<Expense, BusinessException>>,
         IRequestHandler<GetAllInvoicesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetMonthInvoicesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetLastMonthInvoicesQuery, Result<List<Expense>, BusinessException>>
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

        //public async Task<Result<Expense, BusinessException>> Handle(RegisterInvoiceCommand request, CancellationToken cancellationToken)
        //{
        //    return await _invoiceAppService.GetMonthInvoices();
        //}

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetAllInvoices();
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetMonthInvoices();
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetLastMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetLastMonthInvoices();
        }
    }
}

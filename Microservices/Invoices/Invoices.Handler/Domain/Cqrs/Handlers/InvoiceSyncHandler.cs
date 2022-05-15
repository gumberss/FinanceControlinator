using AutoMapper;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models.Sync;
using Invoices.DTOs.Invoices.Sync;
using Invoices.Handler.Domain.Cqrs.Events.Sync;
using MediatR;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;

namespace Invoices.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceSyncHandler : IRequestHandler<InvoiceSyncQuery, Result<InvoiceSyncDTO, BusinessException>>
    {
        private readonly IInvoiceSyncAppService _invoiceSyncAppService;
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ILocalization _localization;
        private readonly ILogger<InvoiceSyncHandler> _logger;
        private readonly IMapper _mapper;

        public InvoiceSyncHandler(
            IInvoiceSyncAppService invoiceSyncAppService
            , IAsyncDocumentSession documentSession
            , ILocalization localization
            , ILogger<InvoiceSyncHandler> logger
            , IMapper mapper)
        {
            _invoiceSyncAppService = invoiceSyncAppService;
            _documentSession = documentSession;
            _localization = localization;
            _logger = logger;
            _mapper = mapper;

        }

        public async Task<Result<InvoiceSyncDTO, BusinessException>> Handle(InvoiceSyncQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceSyncAppService.SyncUpdatesFrom(request.LastSyncTimestamp);

            if (result.IsFailure) return result.Error;

            return _mapper.Map<InvoiceSync, InvoiceSyncDTO>(result.Value);
        }
    }
}

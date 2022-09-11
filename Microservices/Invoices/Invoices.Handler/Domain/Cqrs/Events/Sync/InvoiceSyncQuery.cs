using CleanHandling;
using Invoices.DTOs.Invoices.Sync;
using MediatR;

namespace Invoices.Handler.Domain.Cqrs.Events.Sync
{
    public class InvoiceSyncQuery : IRequest<Result<InvoiceDataSyncDTO, BusinessException>>
    {
        public long LastSyncTimestamp { get; set; }

        public InvoiceSyncQuery(long lastSyncTimestamp)
        {
            LastSyncTimestamp = lastSyncTimestamp;
        }
    }
}

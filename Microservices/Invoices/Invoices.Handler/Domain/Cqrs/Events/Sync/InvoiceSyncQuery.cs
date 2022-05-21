using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.DTOs.Invoices.Sync;
using MediatR;

namespace Invoices.Handler.Domain.Cqrs.Events.Sync
{
    public class InvoiceSyncQuery : IRequest<Result<InvoiceSyncDTO, BusinessException>>
    {
        public long LastSyncTimestamp { get; set; }

        public InvoiceSyncQuery(long lastSyncTimestamp)
        {
            LastSyncTimestamp = lastSyncTimestamp;
        }
    }
}

using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Domain.Models.Sync;
using System.Threading.Tasks;

namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceSyncAppService
    {
        Task<Result<InvoiceSync, BusinessException>> SyncUpdatesFrom(long lastSyncTimestamp);
    }
}

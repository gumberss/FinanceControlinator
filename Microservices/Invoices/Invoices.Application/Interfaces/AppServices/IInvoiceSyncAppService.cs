namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceSyncAppService
    {
        object SyncUpdatesFrom(long lastSyncTimestamp);
    }
}

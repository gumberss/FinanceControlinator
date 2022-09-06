using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceDataSync
    {
        public string SyncName { get; set; }
        public long SyncDate { get; set; }
        public List<InvoiceMonthDataSync> MonthDataSyncs { get; set; }
      
        public InvoiceDataSync(string syncName, long syncDate, List<InvoiceMonthDataSync> monthDataSyncs)
        {
            SyncName = syncName;
            SyncDate = syncDate;
            MonthDataSyncs = monthDataSyncs;
        }
    }
}

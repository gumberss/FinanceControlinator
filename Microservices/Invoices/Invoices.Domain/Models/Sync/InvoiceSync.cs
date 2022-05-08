using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceSync
    {
        public List<InvoiceMonthDataSync> MonthDataSyncs { get; set; }

        public InvoiceSync(List<InvoiceMonthDataSync> monthDataSyncs)
        {
            MonthDataSyncs = monthDataSyncs;
        }
    }
}

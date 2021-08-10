using FinanceControlinator.Events.Invoices.DTOs;
using System.Collections.Generic;

namespace FinanceControlinator.Events.Invoices
{
    public class InvoicesChangedEvent 
    {
        public List<InvoiceDTO> Invoices { get; set; }
    }
}

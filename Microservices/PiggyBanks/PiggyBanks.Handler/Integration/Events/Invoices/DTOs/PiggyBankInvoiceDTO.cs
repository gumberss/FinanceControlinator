using System;
using System.Collections.Generic;

namespace PiggyBanks.Handler.Integration.Events.Invoices.DTOs
{
    public class PiggyBankInvoiceDTO
    {

        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; }

        public DateTime DueDate { get; set; }

        public virtual List<PiggyBankInvoiceItemDTO> Items { get; set; }
    
    }
}

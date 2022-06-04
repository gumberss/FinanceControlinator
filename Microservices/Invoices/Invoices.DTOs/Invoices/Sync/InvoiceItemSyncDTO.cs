using Invoices.DTOs.Invoices.Enum;
using System;

namespace Invoices.DTOs.Invoices.Sync;

public record InvoiceItemSyncDTO
{
    public String Id { get; set; }

    public String InstallmentNumber { get; set; }

    public String InstallmentCost { get; set; }

    public InvoiceItemType Type { get; set; }

    public String PurchaseDay { get; set; }

    public String Title { get; set; }
}


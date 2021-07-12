using Invoices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.API.ViewModels
{
    public class InvoiceItemViewModel
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid InvoiceId { get; set; }

    }
}

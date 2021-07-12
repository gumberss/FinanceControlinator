using Invoices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.API.ViewModels
{
    public class InvoiceViewModel
    {
        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public InvoiceType Type { get; init; }

        public bool IsRecurrent { get; init; } //Monthly only yet

        public String Location { get; init; }

        public String Observation { get; init; }

        public decimal TotalCost { get; set; }

        public List<InvoiceItemViewModel> Items { get; init; }
    }
}

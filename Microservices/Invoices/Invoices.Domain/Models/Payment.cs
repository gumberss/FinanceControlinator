using FinanceControlinator.Common.Entities;
using System;

namespace Invoices.Domain.Models
{
    public class Payment : Entity<String>
    {
        public String Description { get; set; }

        public DateTime Date { get; set; }

        public String ItemId { get; set; }

        public String DetailsPath { get; set; }
    }
}

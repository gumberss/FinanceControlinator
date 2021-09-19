using FinanceControlinator.Common.Entities;
using PiggyBanks.Domain.Enums;
using System;

namespace PiggyBanks.Domain.Models
{
    public class PiggyBank : Entity<Guid>
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Date { get; set; }

        public PiggyBankType Type { get; set; }

        public bool IsRecurrent { get; set; } 

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

    }
}

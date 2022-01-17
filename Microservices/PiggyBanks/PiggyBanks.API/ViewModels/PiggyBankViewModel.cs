using PiggyBanks.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PiggyBanks.API.ViewModels
{
    public class PiggyBankViewModel
    {
        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public PiggyBankType Type { get; init; }

        public bool IsRecurrent { get; init; } //Monthly only yet

        public String Location { get; init; }

        public String Observation { get; init; }

        public decimal TotalCost { get; set; }

        public List<PiggyBankItemViewModel> Items { get; init; }
    }
}

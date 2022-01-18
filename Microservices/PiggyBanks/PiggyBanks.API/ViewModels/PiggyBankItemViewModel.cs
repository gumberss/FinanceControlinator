using System;

namespace PiggyBanks.API.ViewModels
{
    public class PiggyBankItemViewModel
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid PiggyBankId { get; set; }

    }
}

using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounts.API.ViewModels
{
    public class AccountViewModel
    {
        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public AccountType Type { get; init; }

        public bool IsRecurrent { get; init; } //Monthly only yet

        public String Location { get; init; }

        public String Observation { get; init; }

        public decimal TotalCost { get; set; }

        public List<AccountItemViewModel> Items { get; init; }
    }
}

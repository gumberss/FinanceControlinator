using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.ViewModels
{
    public class AccountItemViewModel
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid AccountId { get; set; }

    }
}

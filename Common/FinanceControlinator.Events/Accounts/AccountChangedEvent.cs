using FinanceControlinator.Events.Accounts.DTOs;
using System.Collections.Generic;

namespace FinanceControlinator.Events.Accounts
{
    public class AccountChangedEvent
    {
        public List<AccountChangeDTO> AccountChangeDTOs { get; set; }
    }
}

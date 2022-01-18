using Accounts.Domain.Models;
using System.Collections.Generic;

namespace Accounts.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        bool AbleToPay(Payment paymentRequested, List<Account> accounts);

        List<AccountChange> Pay(Payment paymentRequested, List<Account> accounts);
    }
}

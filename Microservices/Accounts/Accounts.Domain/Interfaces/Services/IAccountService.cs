using Accounts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        bool AbleToPay(Payment paymentRequested, List<Account> accounts);

        List<AccountChange> Pay(Payment paymentRequested, List<Account> accounts);
    }
}

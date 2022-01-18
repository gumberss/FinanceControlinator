using Accounts.Domain.Interfaces.Services;
using Accounts.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounts.Domain.Services
{
    public class AccountService : IAccountService
    {
        public bool AbleToPay(Payment paymentRequested, List<Account> accounts)
        => paymentRequested.PaymentMethods.All(pm =>
                accounts?.Find(x => x.Id == pm.AmountSourceId)?.AbleToPay(pm.Amount) ?? false
            );

        public List<AccountChange> Pay(Payment paymentRequested, List<Account> accounts)
        {
            List<AccountChange> accountChanges = new List<AccountChange>(paymentRequested.PaymentMethods.Count);

            foreach (var paymentMethod in paymentRequested.PaymentMethods)
            {
                var payingAccount = accounts.Find(x => x.Id == paymentMethod.AmountSourceId);

                var change = payingAccount.Withdraw(paymentMethod.Amount, paymentRequested.Id);

                accountChanges.Add(change);
            }

            return accountChanges;
        }
    }
}

using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System;

namespace Accounts.Handler.Domain.Cqrs.Events.Commands
{
    public class AccountWithdrawMoneyCommand : IRequest<Result<Account, BusinessException>>
    {
        public String Description { get; set; }
        public decimal Amount { get; set; }
    }
}

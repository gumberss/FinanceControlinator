using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Accounts.Handler.Domain.Cqrs.Events.Commands
{
    public class AccountReceiveMoneyCommand : IRequest<Result<Account, BusinessException>>
    {
        public decimal Amount { get; set; }
    }
}

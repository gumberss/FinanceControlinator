using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Accounts.Handler.Domain.Cqrs.Events.Commands.Payments
{
    public class PayCommand : IRequest<Result<List<AccountChange>, BusinessException>>
    {
        public Payment Payment { get; set; }
    }
}

using Accounts.Domain.Models;
using CleanHandling;
using MediatR;
using System.Collections.Generic;

namespace Accounts.Handler.Domain.Cqrs.Events.Commands.Payments
{
    public class PayCommand : IRequest<Result<List<AccountChange>, BusinessException>>
    {
        public Payment Payment { get; set; }
    }
}

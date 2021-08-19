using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Accounts.Handler.Domain.Cqrs.Events.Queries
{
    public class AccountDataQuery : IRequest<Result<List<Account>, BusinessException>>
    {
    }
}

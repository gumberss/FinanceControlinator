using Accounts.Domain.Models;
using CleanHandling;
using MediatR;
using System.Collections.Generic;

namespace Accounts.Handler.Domain.Cqrs.Events.Queries
{
    public class AccountDataQuery : IRequest<Result<List<Account>, BusinessException>>
    {
    }
}

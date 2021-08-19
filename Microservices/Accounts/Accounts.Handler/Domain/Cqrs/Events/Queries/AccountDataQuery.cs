using Accounts.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Accounts.Handler.Domain.Cqrs.Events.Queries
{
    public class AccountDataQuery : IRequest<Result<Account, BusinessException>>
    {
    }
}

using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Payments.Domain.Models;
using System.Collections.Generic;

namespace Payments.Handler.Domain.Cqrs.Events.Queries
{
    public class ClosedItemsQuery : IRequest<Result<List<PaymentItem>, BusinessException>>
    {
    }
}

using CleanHandling;
using MediatR;
using Payments.Domain.Models;
using System.Collections.Generic;

namespace Payments.Handler.Domain.Cqrs.Events.Queries
{
    public class ClosedItemsQuery : IRequest<Result<List<PaymentItem>, BusinessException>>
    {
    }
}

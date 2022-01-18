using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class GetLastMonthInvoicesQuery : IRequest<Result<List<Invoice>, BusinessException>>
    {
    }
}

using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Domain.Models;
using MediatR;
using System.Collections.Generic;


namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class GetMonthInvoicesQuery : IRequest<Result<List<Invoice>, BusinessException>>
    {
    }
}

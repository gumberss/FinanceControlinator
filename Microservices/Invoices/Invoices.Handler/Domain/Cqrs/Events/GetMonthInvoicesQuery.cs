using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;


namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class GetMonthInvoicesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
    }
}

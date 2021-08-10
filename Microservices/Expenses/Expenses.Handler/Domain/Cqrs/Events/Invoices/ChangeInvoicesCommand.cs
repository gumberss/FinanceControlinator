using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.Events.Invoices
{
    public class ChangeInvoicesCommand : IRequest<Result<List<Invoice>, BusinessException>>
    {
        public List<Invoice> Invoices { get; set; }
    }
}

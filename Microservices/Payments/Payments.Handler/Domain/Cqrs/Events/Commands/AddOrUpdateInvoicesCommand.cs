using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Payments.Domain.Models;
using System.Collections.Generic;

namespace Payments.Handler.Domain.Cqrs.Events.Commands
{
    public class AddOrUpdateInvoicesCommand : IRequest<Result<List<Invoice>, BusinessException>>
    {
        public List<Invoice> Invoices { get; set; }
    }
}

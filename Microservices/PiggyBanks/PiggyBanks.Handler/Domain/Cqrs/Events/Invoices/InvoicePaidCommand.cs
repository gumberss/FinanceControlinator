using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using PiggyBanks.Domain.Models;
using System.Collections.Generic;

namespace PiggyBanks.Handler.Domain.Cqrs.Events.Invoices
{
    public class InvoicePaidCommand : IRequest<Result<Invoice, BusinessException>>
    {
        public Invoice Invoice { get; set; }
    }
}

using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class RegisterInvoiceCommand : IRequest<Result<Expense, BusinessException>>
    {
        public Expense Invoice { get; set; }
    }
}

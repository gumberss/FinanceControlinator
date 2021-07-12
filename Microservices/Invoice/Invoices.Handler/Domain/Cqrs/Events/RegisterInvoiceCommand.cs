using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class RegisterInvoiceCommand : IRequest<Result<Invoice, BusinessException>>
    {
        public Invoice Invoice { get; set; }
    }
}

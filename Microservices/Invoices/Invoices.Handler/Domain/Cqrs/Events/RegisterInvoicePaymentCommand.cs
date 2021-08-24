using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Domain.Models;
using MediatR;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class RegisterInvoicePaymentCommand : IRequest<Result<Invoice, BusinessException>>
    {
        public Payment Payment { get; set; }
    }
}

using CleanHandling;
using Expenses.Domain.Models.Invoices;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events.Invoices
{
    public class RegisterPaidInvoiceCommand : IRequest<Result<Invoice, BusinessException>>
    {
        public Invoice Invoice { get; set; }
    }
}

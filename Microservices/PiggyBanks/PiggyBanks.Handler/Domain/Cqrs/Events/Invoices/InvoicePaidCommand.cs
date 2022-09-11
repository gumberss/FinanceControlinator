using CleanHandling;
using MediatR;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Domain.Cqrs.Events.Invoices
{
    public class InvoicePaidCommand : IRequest<Result<Invoice, BusinessException>>
    {
        public Invoice Invoice { get; set; }
    }
}

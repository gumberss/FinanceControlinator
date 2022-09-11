using CleanHandling;
using MediatR;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events.Invoices;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Domain.Cqrs.Handlers
{
    public class InvoicesHandler :
        IRequestHandler<InvoicePaidCommand, Result<Invoice, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IPiggyBankDbContext _piggyBankDbContext;

        public InvoicesHandler(IInvoiceAppService invoiceAppService, IPiggyBankDbContext piggyBankDbContext)
        {
            _invoiceAppService = invoiceAppService;
            _piggyBankDbContext = piggyBankDbContext;
        }

        public async Task<Result<Invoice, BusinessException>> Handle(InvoicePaidCommand request, CancellationToken cancellationToken)
        {
            var changedInvoices = await _invoiceAppService.RegisterPaid(request.Invoice);

            if (changedInvoices.IsFailure) return changedInvoices.Error;

            var saveResult = await _piggyBankDbContext.Commit();

            if (saveResult.IsFailure) return saveResult.Error;

            return changedInvoices;
        }
    }
}

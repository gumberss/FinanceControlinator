using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Invoices;
using Expenses.Handler.Domain.Cqrs.Events.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceHandler
        : IRequestHandler<RegisterPaidInvoiceCommand, Result<Invoice, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IExpenseDbContext _expenseDbContext;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService,
            IExpenseDbContext expenseDbContext
            )
        {
            _invoiceAppService = invoiceAppService;
            _expenseDbContext = expenseDbContext;
        }

        public async Task<Result<Invoice, BusinessException>> Handle(RegisterPaidInvoiceCommand request, CancellationToken cancellationToken)
        {
            var changedInvoices = await _invoiceAppService.RegisterPaid(request.Invoice);

            if (changedInvoices.IsFailure) return changedInvoices.Error;

            var saveResult = await Result.Try(_expenseDbContext.Commit());

            if (saveResult.IsFailure) return saveResult.Error;

            return changedInvoices;
        }
    }
}

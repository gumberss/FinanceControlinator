using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Data.Repositories;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace Expenses.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IExpenseDbContext _expenseDbContext;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public InvoiceAppService(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IExpenseDbContext expenseDbContext
            )
        {
            _invoiceRepository = invoiceRepository;
            _expenseDbContext = expenseDbContext;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<Result<Invoice, BusinessException>> RegisterPaid(Invoice paidInvoice)
            => await _invoiceRepository.GetByIdAsync(paidInvoice.Id)
                    .When(existentInvoice => existentInvoice is not null,
                     @then: existentInvoice => _invoiceItemRepository.DeleteAsync(existentInvoice.Items)
                        .Then(_ => _invoiceItemRepository.AddAsync(paidInvoice.Items))
                        .Then(_ => existentInvoice
                            .ChangeDueDate(paidInvoice.DueDate)
                            .ReplaceItems(paidInvoice.Items))
                        .Then(existentInvoice => _invoiceRepository.UpdateAsync(existentInvoice)),
                     @else: _ => _invoiceRepository.AddAsync(paidInvoice));
    }
}

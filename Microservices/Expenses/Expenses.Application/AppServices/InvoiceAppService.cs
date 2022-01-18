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
        {
            var registeredInvoice = await _invoiceRepository.GetByIdAsync(paidInvoice.Id);

            if (registeredInvoice.IsFailure) return registeredInvoice.Error;

            if (registeredInvoice.Value is not null)
            {
                var deleteItemsResult = await _invoiceItemRepository.DeleteAsync(registeredInvoice.Value.Items);

                if (deleteItemsResult.IsFailure) return deleteItemsResult.Error;

                var addNewItemsResult = await _invoiceItemRepository.AddAsync(paidInvoice.Items);

                if (addNewItemsResult.IsFailure) return addNewItemsResult.Error;

                registeredInvoice.Value
                    .ChangeDueDate(paidInvoice.DueDate)
                    .ReplaceItems(paidInvoice.Items);

                return await _invoiceRepository.UpdateAsync(registeredInvoice.Value);
            }

            return await _invoiceRepository.AddAsync(paidInvoice);
        }
    }
}

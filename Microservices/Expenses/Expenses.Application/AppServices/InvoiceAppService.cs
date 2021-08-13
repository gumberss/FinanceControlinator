using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Contexts;
using Expenses.Data.Repositories;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ExpenseDbContext _expenseDbContext;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public InvoiceAppService(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
                ExpenseDbContext expenseDbContext
            )
        {
            _invoiceRepository = invoiceRepository;
            _expenseDbContext = expenseDbContext;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<Result<Invoice, BusinessException>> RegisterPaid(Invoice paidInvoice)
        {
            var registeredInvoice = await _invoiceRepository.GetByIdAsync(paidInvoice.Id);

            if (registeredInvoice.IsFailure)
            {
                //log
                return registeredInvoice.Error;
            }

            if (registeredInvoice.Value is not null)
            {
                //log
                return registeredInvoice.Value;
            }

            return await _invoiceRepository.AddAsync(paidInvoice);
        }
    }
}

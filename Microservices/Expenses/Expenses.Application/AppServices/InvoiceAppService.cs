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

        public async Task<Result<List<Invoice>, BusinessException>> Change(List<Invoice> changedInvoices)
        {
            var invoicesIds = changedInvoices.Select(x => x.Id);

            var existentInvoices = await _invoiceRepository.GetAllAsync(x => x.Items, x => invoicesIds.Contains(x.Id));

            if (existentInvoices.IsFailure)
            {
                //log
                return existentInvoices.Error;
            }

            var oldInvoiceItems = new List<InvoiceItem>();

            foreach (var existentInvoice in existentInvoices.Value)
            {
                var changedInvoice = changedInvoices.Find(x => x.Id == existentInvoice.Id);

                oldInvoiceItems.AddRange(existentInvoice.Items);

                existentInvoice
                    .ChangeDueDate(changedInvoice.DueDate)
                    .ReplaceItems(changedInvoice.Items);

                var updateResult = await _invoiceRepository.UpdateAsync(existentInvoice);

                if (updateResult.IsFailure)
                {
                    //log
                    return updateResult.Error;
                }
            }

            var newInvoices = changedInvoices
                .Where(x => existentInvoices.Value.All(y => y.Id != x.Id));

            foreach (var item in oldInvoiceItems)
                await _invoiceItemRepository.DeleteAsync(item);

            foreach (var item in existentInvoices.Value.SelectMany(x=> x.Items))
                await _invoiceItemRepository.AddAsync(item);

            foreach (var newInvoice in newInvoices)
                await _invoiceRepository.AddAsync(newInvoice);

            return newInvoices.Union(existentInvoices.Value).ToList();
        }
    }
}

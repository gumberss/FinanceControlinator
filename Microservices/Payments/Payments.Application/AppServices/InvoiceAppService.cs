using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Payments.Domain.Models;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceAppService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Change(List<Invoice> changedInvoices)
        {
            var invoicesIds = changedInvoices.Select(x => x.Id);

            var existentInvoices = await _invoiceRepository.GetAllAsync(x => x.Items, x => x.Id.In(invoicesIds));

            if (existentInvoices.IsFailure)
            {
                //log
                return existentInvoices.Error;
            }

            foreach (var existentInvoice in existentInvoices.Value)
            {
                var changedInvoice = changedInvoices.Find(x => x.Id == existentInvoice.Id);

                existentInvoice
                    .UpdateFrom(changedInvoice)
                    .ReplaceItems(changedInvoice.Items);
            }

            var newInvoices = changedInvoices
                .Where(x => existentInvoices.Value.All(y => y.Id != x.Id));

            foreach (var newInvoice in newInvoices)
                await _invoiceRepository.AddAsync(newInvoice);

            return newInvoices.Union(existentInvoices.Value).ToList();
        }
    }
}

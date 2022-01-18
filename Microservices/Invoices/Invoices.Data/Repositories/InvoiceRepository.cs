using FinanceControlinator.Common.Repositories;
using Invoices.Data.Commons;
using Invoices.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Invoices.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice, String>
    {

    }

    public class InvoiceRepository : Repository<Invoice, String>, IInvoiceRepository
    {
        public InvoiceRepository(IAsyncDocumentSession context) : base(context)
        {

        }
    }
}

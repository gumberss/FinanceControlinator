using FinanceControlinator.Common.Repositories;
using Payments.Data.Commons;
using Payments.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Payments.Data.Repositories
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

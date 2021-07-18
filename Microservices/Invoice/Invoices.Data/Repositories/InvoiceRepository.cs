using Invoices.Data.Commons;
using Invoices.Data.Contexts;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Repositories;
using Raven.Client.Documents.Session;
using System;

namespace Invoices.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Expense, String>
    {

    }

    public class InvoiceRepository : Repository<Expense, String>, IInvoiceRepository
    {
        public InvoiceRepository(IDocumentSession context) : base(context)
        {

        }
    }
}

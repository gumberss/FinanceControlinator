using Invoices.Data.Commons;
using Invoices.Data.Contexts;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Repositories;

namespace Invoices.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {

    }

    public class InvoiceRepository : Repository<Invoice, InvoiceDbContext>, IInvoiceRepository
    {
        public InvoiceRepository(InvoiceDbContext context) : base(context)
        {
            
        }
    }
}

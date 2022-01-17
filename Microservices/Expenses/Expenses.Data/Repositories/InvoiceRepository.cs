using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Repositories;
using System;

namespace Expenses.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
    }

    public class InvoiceRepository : Repository<Invoice, ExpenseDbContext, Guid>, IInvoiceRepository
    {
        public InvoiceRepository(IExpenseDbContext context) : base(context as ExpenseDbContext)
        {

        }
    }
}

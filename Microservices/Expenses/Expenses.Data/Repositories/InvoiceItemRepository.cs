using Expenses.Data.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Repositories;
using System;

namespace Expenses.Data.Repositories
{
    public interface IInvoiceItemRepository : IRepository<InvoiceItem, Guid>
    {

    }

    public class InvoiceItemRepository : Repository<InvoiceItem, ExpenseDbContext, Guid>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IExpenseDbContext context) : base(context as ExpenseDbContext)
        {

        }
    }
}

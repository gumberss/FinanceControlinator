using FinanceControlinator.Common.Repositories;
using Invoices.Data.Commons;
using Invoices.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Invoices.Data.Repositories
{
    public interface IExpenseRepository : IRepository<Expense, String>
    {

    }

    public class ExpenseRepository : Repository<Expense, String>, IExpenseRepository
    {
        public ExpenseRepository(IAsyncDocumentSession context) : base(context)
        {

        }
    }
}
using FinanceControlinator.Common.Repositories;
using Invoices.Data.Commons;
using Invoices.Domain.Models;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
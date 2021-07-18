using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        public Task<Result<Expense, BusinessException>> RegisterExpense(Expense invoice);

        Task<Result<List<Expense>, BusinessException>> GetAllInvoices();

        Task<Result<List<Expense>, BusinessException>> GetMonthInvoices();

        Task<Result<List<Expense>, BusinessException>> GetLastMonthInvoices();
    }
}

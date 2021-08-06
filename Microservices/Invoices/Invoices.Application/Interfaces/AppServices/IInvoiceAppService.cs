using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        public Task<Result<List<Invoice>, BusinessException>> RegisterExpense(Expense invoice);

        Task<Result<List<Invoice>, BusinessException>> GetAllInvoices();

        Task<Result<List<Invoice>, BusinessException>> GetMonthInvoice();

        Task<Result<List<Invoice>, BusinessException>> GetLastMonthInvoice();
    }
}

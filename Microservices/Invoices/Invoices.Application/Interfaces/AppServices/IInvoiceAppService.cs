using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<List<Invoice>, BusinessException>> RegisterInvoiceItems(Expense expense);

        Task<Result<List<Invoice>, BusinessException>> GetAllInvoices();

        Task<Result<List<Invoice>, BusinessException>> GetMonthInvoice();

        Task<Result<List<Invoice>, BusinessException>> GetLastMonthInvoice();
        Task<Result<Invoice, BusinessException>> Pay(Invoice invoice);
    }
}

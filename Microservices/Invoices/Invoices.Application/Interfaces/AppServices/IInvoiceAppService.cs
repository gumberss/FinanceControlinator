using CleanHandling;
using Invoices.Domain.Models;
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
        Task<Result<Invoice, BusinessException>> RegisterPayment(Payment payment);
    }
}

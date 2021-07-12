using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        public Task<Result<Invoice, BusinessException>> RegisterInvoice(Invoice invoice);

        Task<Result<List<Invoice>, BusinessException>> GetAllInvoices();

        Task<Result<List<Invoice>, BusinessException>> GetMonthInvoices();

        Task<Result<List<Invoice>, BusinessException>> GetLastMonthInvoices();
    }
}

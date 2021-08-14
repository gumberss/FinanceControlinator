using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<Invoice, BusinessException>> RegisterPaid(Invoice paidInvoice);
    }
}

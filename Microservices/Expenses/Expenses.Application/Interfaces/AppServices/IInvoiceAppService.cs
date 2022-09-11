using CleanHandling;
using Expenses.Domain.Models.Invoices;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<Invoice, BusinessException>> RegisterPaid(Invoice paidInvoice);
    }
}

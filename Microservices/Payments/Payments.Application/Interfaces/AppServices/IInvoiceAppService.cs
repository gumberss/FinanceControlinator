using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Payments.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<List<Invoice>, BusinessException>> Change(List<Invoice> changedInvoices);
    }
}

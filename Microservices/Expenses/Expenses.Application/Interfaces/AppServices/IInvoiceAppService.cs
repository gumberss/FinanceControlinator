using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IInvoiceAppService
    {
        Task<Result<List<Invoice>, BusinessException>> Change(List<Invoice> invoices);
    }
}

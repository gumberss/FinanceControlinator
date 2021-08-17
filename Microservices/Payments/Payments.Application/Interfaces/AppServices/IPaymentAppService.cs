using Payments.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces.AppServices
{
    public interface IPaymentAppService
    {
        Task<Result<PaymentItem, BusinessException>> RegisterItem(PaymentItem paymentItem);
        Task<Result<Payment, BusinessException>> Pay(string itemId, string description, List<PaymentMethod> paymentMethods);
        Task<Result<List<PaymentItem>, BusinessException>> GetClosedItems();
    }
}

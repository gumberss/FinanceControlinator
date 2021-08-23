using FinanceControlinator.Common.Repositories;
using Payments.Data.Commons;
using Payments.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Payments.Data.Repositories
{
    public interface IPaymentItemRepository : IRepository<PaymentItem, String>
    {

    }

    public class PaymentItemRepository : Repository<PaymentItem, String>, IPaymentItemRepository
    {
        public PaymentItemRepository(IAsyncDocumentSession context) : base(context)
        {

        }
    }
}
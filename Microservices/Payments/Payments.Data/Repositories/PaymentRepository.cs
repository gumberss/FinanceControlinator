using FinanceControlinator.Common.Repositories;
using Payments.Data.Commons;
using Payments.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Payments.Data.Repositories
{
    public interface IPaymentRepository : IRepository<Payment, String>
    {

    }

    public class PaymentRepository : Repository<Payment, String>, IPaymentRepository
    {
        public PaymentRepository(IAsyncDocumentSession context) : base(context)
        {

        }
    }
}

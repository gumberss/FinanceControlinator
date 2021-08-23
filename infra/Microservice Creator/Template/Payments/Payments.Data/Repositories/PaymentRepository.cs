using Payments.Data.Commons;
using Payments.Data.Contexts;
using Payments.Domain.Models;
using FinanceControlinator.Common.Repositories;
using System;
using Raven.Client.Documents.Session;

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

using FinanceControlinator.Common.Repositories;
using Invoices.Data.Commons;
using Invoices.Domain.Models;
using Raven.Client.Documents.Session;
using System;

namespace Invoices.Data.Repositories
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

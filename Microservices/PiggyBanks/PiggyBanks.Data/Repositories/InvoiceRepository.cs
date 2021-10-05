using FinanceControlinator.Common.Repositories;
using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using System;

namespace PiggyBanks.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
    }


    public class InvoiceRepository : Repository<Invoice, PiggyBankDbContext, Guid>, IInvoiceRepository
    {
        public InvoiceRepository(IPiggyBankDbContext context) : base(context as PiggyBankDbContext)
        {

        }
    }
}

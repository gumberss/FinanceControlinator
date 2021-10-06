using FinanceControlinator.Common.Repositories;
using PiggyBanks.Data.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using System;

namespace PiggyBanks.Data.Repositories
{
    public interface IInvoiceItemRepository : IRepository<InvoiceItem, Guid>
    {
    }


    public class InvoiceItemRepository : Repository<InvoiceItem, PiggyBankDbContext, Guid>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IPiggyBankDbContext context) : base(context as PiggyBankDbContext)
        {

        }
    }
}

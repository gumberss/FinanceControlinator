using System.Threading;
using System.Threading.Tasks;

namespace Invoices.Data.Interfaces.Contexts
{
    public interface IInvoiceDbContext
    {
        Task<int> Commit();
    }
}

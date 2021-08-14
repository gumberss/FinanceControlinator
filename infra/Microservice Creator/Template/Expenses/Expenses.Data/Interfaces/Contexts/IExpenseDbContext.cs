using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Data.Interfaces.Contexts
{
    public interface IExpenseDbContext
    {
        Task<int> Commit();
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Data.Interfaces.Contexts
{
    public interface IPiggyBankDbContext
    {
        Task<int> Commit();
    }
}

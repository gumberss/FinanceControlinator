using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Contexts
{
    public interface IContext : IDisposable
    {
        Task<Result<bool, BusinessException>> Commit();
    }
}

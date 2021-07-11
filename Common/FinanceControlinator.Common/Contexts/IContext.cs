using System;

namespace FinanceControlinator.Common.Contexts
{
    public interface IContext : IDisposable
    {
        void Commit();
    }
}

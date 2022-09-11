using CleanHandling;
using FinanceControlinator.Common.Contexts;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Net;
using System.Threading.Tasks;

namespace Invoices.Data.Contexts
{
    public class DocumentoStoreContext : IContext
    {
        private readonly IDocumentStore _store;
        private IAsyncDocumentSession _session;
        private bool _disposed;

        public DocumentoStoreContext(IDocumentStore store)
            => _store = store;

        public async Task<Result<bool, BusinessException>> Commit()
            => _session is not null
            ? await Result.Try(_session.SaveChangesAsync())
            : Result.FromError<bool>(new BusinessException(HttpStatusCode.InternalServerError, "Session not found"));

        public void Dispose()
        {
            if (_disposed) return;

            _session?.Dispose();

            _disposed = true;
        }

        public IAsyncDocumentSession Context()
            => _session = _store.OpenAsyncSession();
    }
}

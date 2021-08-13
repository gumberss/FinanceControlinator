using System;
using Raven.Client.Documents;

namespace Payments.Data.Contexts
{
    public static class DocumentStoreHolder
    {
        private static IDocumentStore _store;

        public static IDocumentStore GetStore(string connection, String database)
        {
            if (_store is null)
            {
                var store = new DocumentStore
                {
                    Urls = new[] { connection },
                    Database = database
                };

                _store = store.Initialize();
            }

            return _store;
        }
    }
}

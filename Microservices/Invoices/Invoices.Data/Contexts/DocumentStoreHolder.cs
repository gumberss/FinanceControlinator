using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System;

namespace Invoices.Data.Contexts
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

                EnsureDatabaseExists(_store, database);
            }

            return _store;
        }

        public static void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (!createDatabaseIfNotExists)
                    throw;

                try
                {
                    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                    // The database was already created before calling CreateDatabaseOperation
                }

            }
        }
    }
}

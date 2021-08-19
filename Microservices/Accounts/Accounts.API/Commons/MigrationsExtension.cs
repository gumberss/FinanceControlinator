using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Accounts.API.Commons
{
    public static class MigrationsExtension
    {
        //public static IHost EnsureDatabaseExists(this IHost host)
        //{
        //    var serviceScopeFactory = host.Services.GetService<IServiceScopeFactory>();

        //    using (var scope = serviceScopeFactory.CreateScope())
        //    {
        //        var store = (IDocumentStore)scope.ServiceProvider.GetService(typeof(IDocumentStore));

        //        var database = store.Database;

        //        if (string.IsNullOrWhiteSpace(database))
        //            throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

        //        try
        //        {
        //            store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
        //        }
        //        catch (DatabaseDoesNotExistException)
        //        {
        //            try
        //            {
        //                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
        //            }
        //            catch (ConcurrencyException)
        //            {
        //                // The database was already created before calling CreateDatabaseOperation
        //            }
        //        }
        //    }

        //    return host;
        //}
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace PiggyBanks.API.Commons
{
    public static class MigrationsExtension
    {
        public static async Task<IHost> Migrate(this IHost host)
        {
            var serviceScopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                using (var context = (PiggyBankDbContext)scope.ServiceProvider.GetService(typeof(IPiggyBankDbContext)))
                {
                    var needMigration = (await context.Database.GetPendingMigrationsAsync()).Any();

                    if (needMigration) context.Database.Migrate();

                    return host;
                }
            }


        }
    }
}

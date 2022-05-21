using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Commons
{
    public static class MigrationsExtension
    {
        public static async Task<T> Migrate<T>(this T host) where T : IHost
        {
            var serviceScopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                using (var context = (ExpenseDbContext)scope.ServiceProvider.GetService(typeof(IExpenseDbContext)))
                {
                    var needMigration = (await context.Database.GetPendingMigrationsAsync()).Any();

                    if (needMigration) context.Database.Migrate();

                    return host;
                }
            }
        }
    }
}

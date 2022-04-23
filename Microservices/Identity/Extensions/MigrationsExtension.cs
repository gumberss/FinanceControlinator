using Identity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Extensions
{
    public static class MigrationsExtension
    {
        public static async Task<T> Migrate<T>(this T host) where T : IHost
        {
            var serviceScopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                using (var context = (scope.ServiceProvider.GetService(typeof(IdentityAppDbContext)) as IdentityAppDbContext))
                {
                    var needMigration = (await context!.Database.GetPendingMigrationsAsync()).Any();

                    if (needMigration) context.Database.Migrate();

                    return host;
                }
            }
        }
    }
}

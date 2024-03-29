﻿using Expenses.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Commons
{
    public static class MigrationsExtension
    {
        public static async Task<IHost> Migrate(this IHost host)
        {
            var serviceScopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                using (var context = (ExpenseDbContext)scope.ServiceProvider.GetService(typeof(ExpenseDbContext)))
                {
                    var needMigration = (await context.Database.GetPendingMigrationsAsync()).Any();

                    if (needMigration) context.Database.Migrate();

                    return host;
                }
            }

           
        }
    }
}

using Accounts.Application.AppServices;
using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Accounts.Domain.Localizations;
using Accounts.Handler.Configurations;
using Accounts.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.LogsBehaviors;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Accounts.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(AccountHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IAccountAppService, AccountAppService>();
            
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddSingleton<CosmosClient>(InitializeCosmosClientInstanceAsync(configuration.GetSection("DbConnection")).GetAwaiter().GetResult());
        }

        private static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var account = configurationSection["EndpointUri"];
            var key = configurationSection["PrimaryKey"];
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            return client;
        }
    }
}

using Accounts.Application.AppServices;
using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Accounts.Domain.Interfaces.Services;
using Accounts.Domain.Localizations;
using Accounts.Domain.Services;
using Accounts.Handler.Configurations;
using Accounts.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.LogsBehaviors;
using FinanceControlinator.Common.Messaging;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Accounts.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(AccountHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IAccountAppService, AccountAppService>();
            
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddSingleton<CosmosClient>(InitializeCosmosClientInstanceAsync(configurationSection).GetAwaiter().GetResult());
        }

        private static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value; 
            string account = configurationSection.GetSection("EndpointUri").Value;
            string key = configurationSection.GetSection("PrimaryKey").Value;

            CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return client;
        }
    }
}

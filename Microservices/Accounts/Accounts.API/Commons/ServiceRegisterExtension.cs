using Accounts.Application.AppServices;
using Accounts.Application.Interfaces.AppServices;
using Accounts.Data.Repositories;
using Accounts.Domain.Localizations;
using Accounts.Handler.Configurations;
using Accounts.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.LogsBehaviors;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(AccountHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IAccountAppService, AccountAppService>();
            
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}

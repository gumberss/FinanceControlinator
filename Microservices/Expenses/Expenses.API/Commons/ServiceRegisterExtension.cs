using Expenses.Application.AppServices;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Validators;
using Expenses.Handler.Configurations;
using Expenses.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.CustomLogs;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FinanceControlinator.Common.LogsBehaviors;

namespace Expenses.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(ExpenseHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IExpenseAppService, ExpenseAppService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IExpenseValidator, ExpenseValidator>();
        }
    }
}

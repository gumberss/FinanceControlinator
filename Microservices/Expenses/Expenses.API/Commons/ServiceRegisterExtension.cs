using Expenses.Application.AppServices;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Services;
using Expenses.Domain.Validators;
using Expenses.Handler.Configurations;
using Expenses.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.LogsBehaviors;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Common.Utils;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<IMessageBus, MassTransitMessageBus>();

            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddTransient<IDateService, DateService>();

            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<IExpenseOverviewService, ExpenseOverviewService>();
            services.AddTransient<ITextParser, TextParser>();
            
            services.AddTransient<IExpenseAppService, ExpenseAppService>();
            services.AddTransient<IExpenseOverviewAppService, ExpenseOverviewAppService>();
            services.AddTransient<IInvoiceAppService, InvoiceAppService>();

            services.AddTransient<IExpenseItemRepository, ExpenseItemRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();

            services.AddTransient<IExpenseValidator, ExpenseValidator>();
        }
    }
}

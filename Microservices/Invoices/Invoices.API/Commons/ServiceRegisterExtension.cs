using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Handler.Configurations;
using Invoices.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.CustomLogs;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Invoices.Domain.Services;
using FinanceControlinator.Common.LogsBehaviors;

namespace Invoices.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(InvoiceHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddTransient<IInvoiceAppService, InvoiceAppService>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IInvoiceService, InvoiceService>();

            /*
                IDocumentStore documentStore
                , IAsyncDocumentSession documentSession
                , IInvoiceValidator invoiceValidator
                , ILocalization localization
                , ILogger<IInvoiceAppService> logger
                , IInvoiceService invoiceService
             */
        }
    }
}

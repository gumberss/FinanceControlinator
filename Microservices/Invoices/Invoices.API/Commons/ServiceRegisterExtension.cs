using FinanceControlinator.Common.LogsBehaviors;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Common.Utils;
using FluentValidation.AspNetCore;
using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Services;
using Invoices.Handler.Configurations;
using Invoices.Handler.Domain.Cqrs.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Invoices.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(InvoiceHandler));

            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddTransient<IInvoiceSyncAppService, InvoiceSyncAppService>();
            services.AddTransient<IInvoiceAppService, InvoiceAppService>();

            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IInvoiceOverviewService, InvoiceOverviewService>();
            services.AddTransient<IInvoiceSyncService, InvoiceSyncService>();

            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            services.AddTransient<ITextParser, TextParser>();
            services.AddTransient<IDateService, DateService>();
            services.AddTransient<IMessageBus, MassTransitMessageBus>();

        }
    }
}

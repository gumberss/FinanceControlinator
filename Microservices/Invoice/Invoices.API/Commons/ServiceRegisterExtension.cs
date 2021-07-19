using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Interfaces.Validators;
using Invoices.Domain.Localizations;
using Invoices.Domain.Validators;
using Invoices.Handler.Configurations;
using Invoices.Handler.Domain.Cqrs.Handlers;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.LogsBehaviors;
using FluentValidation.AspNetCore;
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

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(InvoiceHandler));
            
            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddTransient<IInvoiceAppService, InvoiceAppService>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceValidator, InvoiceValidator>();
        }
    }
}

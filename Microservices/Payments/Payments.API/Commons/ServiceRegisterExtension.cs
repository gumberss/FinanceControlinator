using FinanceControlinator.Common.LogsBehaviors;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.AppServices;
using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Localizations;
using Payments.Domain.Validators;
using Payments.Handler.Configurations;
using Payments.Handler.Domain.Cqrs.Handlers;

namespace Payments.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(PaymentHandler));

            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<IInvoiceAppService, InvoiceAppService>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentItemRepository, PaymentItemRepository>();

            services.AddTransient<IInvoiceRepository, InvoiceRepository>();

            services.AddTransient<IPaymentItemValidator, PaymentItemValidator>();
        }
    }
}

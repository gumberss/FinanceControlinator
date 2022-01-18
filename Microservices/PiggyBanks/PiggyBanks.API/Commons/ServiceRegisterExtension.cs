using FinanceControlinator.Common.LogsBehaviors;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PiggyBanks.Application.AppServices;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Interfaces.Validators;
using PiggyBanks.Domain.Localizations;
using PiggyBanks.Domain.Validators;
using PiggyBanks.Handler.Domain.Cqrs.Handlers;

namespace PiggyBanks.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.ConfigureHandlerAutoMapper();

            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(PiggyBankHandler));

            services.AddTransient<ILocalization, Ptbr>();

            services.AddFluentValidation();

            services.AddScoped<IPiggyBankAppService, PiggyBankAppService>();
            services.AddScoped<IPiggyBankRepository, PiggyBankRepository>();
            services.AddScoped<IPiggyBankValidator, PiggyBankValidator>();

            services.AddScoped<IInvoiceAppService, InvoiceAppService>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();

            services.AddScoped<ITransferAppService, TransferAppService>();
            services.AddScoped<ITransferRepository, TransferRepository>();
        }
    }
}

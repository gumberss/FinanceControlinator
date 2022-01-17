using Accounts.Handler.Integration.Handlers;
using FinanceControlinator.Common.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Accounts.Handler.Configurations
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(this IServiceCollection services, RabbitMqValues configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AccountsPaymentIntegrationHandler>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration.Host), host =>
                    {
                        host.Username(configuration.Username);
                        host.Password(configuration.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            services.AddScoped<IMessageBus, MassTransitMessageBus>();
        }
    }
}

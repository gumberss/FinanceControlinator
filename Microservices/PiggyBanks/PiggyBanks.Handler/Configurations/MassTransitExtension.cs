using FinanceControlinator.Events.PiggyBanks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PiggyBanks.Handler.Integration.Handlers;
using System;

namespace PiggyBanks.Handler.Configurations
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(this IServiceCollection services, RabbitMqValues configuration)
        {
           services.AddMassTransit(x =>
           {
               x.AddConsumer<PiggyBanksIntegrationHandler>();
               x.AddConsumer<PiggyBanksInvoiceIntegrationHandler>();

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
        }
    }
}

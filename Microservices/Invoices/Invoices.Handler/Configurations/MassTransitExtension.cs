using Invoices.Handler.Integration.Handlers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Invoices.Handler.Configurations
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(this IServiceCollection services, RabbitMqValues configuration)
        {
           services.AddMassTransit(x =>
           {
               x.AddConsumer<InvoiceIntegrationHandler>();

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

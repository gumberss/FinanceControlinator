using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Payments.Handler.Integration.Handlers;
using System;

namespace Payments.Handler.Configurations
{
    public static class MassTransitExtension
    {
        public static void ConfigureMassTransit(this IServiceCollection services, RabbitMqValues configuration)
        {
           services.AddMassTransit(x =>
           {
               x.AddConsumer<PaymentIntegrationHandler>();

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

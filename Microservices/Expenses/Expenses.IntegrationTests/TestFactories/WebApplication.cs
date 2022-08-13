using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit.Testing;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Expenses.Data.Interfaces.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Expenses.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using FinanceControlinator.Authentication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Expenses.IntegrationTests.TestFactories
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ExpenseDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IExpenseDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    ((ExpenseDbContext)db).Database.EnsureCreated();
                }
            });
        }
    }


    public class WebApplication
    {
        public HttpClient Mock2(Guid userId, params IConsumer[] consumers)
        {
            IConfiguration config = null;

            var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
                       {
                           options.UseInMemoryDatabase("InMemoryDbForTesting");
                       });

                       var result = new MassTransitFacker().ConfigureMassTransit(services, consumers).Result;

                       var sp = services.BuildServiceProvider();

                       using (var scope = sp.CreateScope())
                       {
                           var scopedServices = scope.ServiceProvider;
                           config = scopedServices.GetRequiredService<IConfiguration>();
                           var db = scopedServices.GetRequiredService<IExpenseDbContext>();
                           ((ExpenseDbContext)db).Database.EnsureCreated();
                       }
                   });

               });
            var client = application.CreateClient();
            Authorize(client, userId, config!);
            return client;
        }
        public HttpClient Authorize(HttpClient client, Guid userId, IConfiguration config)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", new TokenService()
                .BuildToken(config["Jwt:Key"],
                                 config["Jwt:Issuer"],
                                 new[] { config["Jwt:Audience"]},
                                 "TestUser",
                                 userId.ToString(),
                                 TimeSpan.FromMinutes(10)));

            return client;
        }
    }
}

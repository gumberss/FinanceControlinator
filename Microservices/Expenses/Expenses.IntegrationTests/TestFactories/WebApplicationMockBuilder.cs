using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using MassTransit;
using Expenses.Data.Interfaces.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Expenses.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using FinanceControlinator.Authentication.Services;
using Microsoft.Extensions.Configuration;
using MassTransit.Testing;

namespace Expenses.IntegrationTests.TestFactories
{
    public class WebApplicationMockBuilder
    {
        private Guid _userId;
        private IConsumer[] _consumers;

        public WebApplicationMockBuilder()
        {

        }

        public WebApplicationMockBuilder WithAuthorizedUser(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public WebApplicationMockBuilder WithConsumers(params IConsumer[] consumers)
        {
            _consumers = consumers;
            return this;
        }

        public (HttpClient, InMemoryTestHarness) Build()
        {
            IConfiguration config = null;
            InMemoryTestHarness harness = null;

            var application = new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.ConfigureServices(services =>
                  {
                      services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
                      {
                          options.UseInMemoryDatabase("InMemoryDbForTesting");
                      });

                      harness = new MassTransitFacker().ConfigureMassTransit(services, _consumers).Result;

                      var sp = services.BuildServiceProvider(true);

                      using var scope = sp.CreateScope();
                      var scopedServices = scope.ServiceProvider;

                      config = scopedServices.GetRequiredService<IConfiguration>();
                      var db = scopedServices.GetRequiredService<IExpenseDbContext>();
                      ((ExpenseDbContext)db).Database.EnsureCreated();
                  });
              });

            var client = application.CreateClient();
            Authorize(client, _userId, config!);
            return (client, harness!);
        }

        public static HttpClient Authorize(HttpClient client, Guid userId, IConfiguration config)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", new TokenService()
                .BuildToken(config["Jwt:Key"],
                                 config["Jwt:Issuer"],
                                 new[] { config["Jwt:Audience"] },
                                 "TestUser",
                                 userId.ToString(),
                                 TimeSpan.FromMinutes(10)));

            return client;
        }
    }
}

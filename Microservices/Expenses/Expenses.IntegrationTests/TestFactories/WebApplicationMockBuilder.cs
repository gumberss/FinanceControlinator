using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using FinanceControlinator.Authentication.Services;
using Microsoft.Extensions.Configuration;
using MassTransit.Testing;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Expenses.IntegrationTests.TestFactories
{
    public interface IFakeConfig
    {
        Task Configure(IServiceCollection services);

    }

    public interface IFakeConfig<TResult> : IFakeConfig where TResult : class
    {
        TResult Get(IServiceProvider provider);
    }

    public class WebApplicationMockBuilder : IDisposable
    {
        private Guid _userId;
        private List<IFakeConfig> _fakeConfigs;
        private IServiceScope _scope;

        private bool _disposed;

        public WebApplicationMockBuilder()
        {
            _fakeConfigs = new List<IFakeConfig>();
        }

        public WebApplicationMockBuilder WithAuthorizedUser(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public WebApplicationMockBuilder AddServiceConfiguration<TResult>(IFakeConfig<TResult> fakeConfig) where TResult : class
        {
            _fakeConfigs.Add(fakeConfig);

            return this;
        }

        /// <summary>
        /// Call After Build!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult? GetFake<TResult>() where TResult : class
            => _fakeConfigs.OfType<IFakeConfig<TResult>>()
                    .FirstOrDefault()
                    ?.Get(_scope.ServiceProvider);

        /// <summary>
        /// Call After Build!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult? GetService<TResult>() where TResult : class
            => _scope.ServiceProvider.GetService<TResult>();

        public HttpClient Build()
        {
            IConfiguration config = null;

            var application = new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.UseEnvironment("Test");
                  builder.ConfigureServices(services =>
                  {
                      _fakeConfigs.ForEach(x => x.Configure(services));

                      var sp = services.BuildServiceProvider(true);

                      _scope = sp.CreateScope();
                      var scopedServices = _scope.ServiceProvider;
                      config = scopedServices.GetRequiredService<IConfiguration>();

                  });
              });

            var client = application.CreateClient();
            Authorize(client, _userId, config!);
            return client;
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

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            _scope.Dispose();
        }
    }
}

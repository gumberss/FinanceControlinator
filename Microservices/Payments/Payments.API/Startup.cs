using FinanceControlinator.Common.CustomLogs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Payments.API.Commons;
using Payments.Data.Contexts;
using Payments.Handler.Configurations;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Payments.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var rabbitMqValues = new RabbitMqValues()
            {
                Host = Configuration.GetSection("RabbitMq:Host").Value,
                Username = Configuration.GetSection("RabbitMq:Username").Value,
                Password = Configuration.GetSection("RabbitMq:Password").Value,
            };

            services.ConfigureMassTransit(rabbitMqValues);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            var dbConnection = Configuration.GetConnectionString("PaymentsDbConnection");
            var dbName = Configuration.GetConnectionString("PaymentsDbName");
            services.AddSingleton<IDocumentStore>(x => DocumentStoreHolder.GetStore(dbConnection, dbName));
            services.AddScoped<IAsyncDocumentSession>(x => x.GetService<IDocumentStore>().OpenAsyncSession());

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Payments Microservice", Version = "v1" });
            });

            services.AddControllers(x => x.UseCentralRoutePrefix(new RouteAttribute("api/")));

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
            , IWebHostEnvironment env
            , ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new CustomLogProvider(new CustomLogConfig()));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments Microservice V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

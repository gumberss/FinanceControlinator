using Invoices.API.Commons;
using Invoices.Data.Contexts;
using Invoices.Handler.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using MassTransit;
using Invoices.Handler.Integration.Handlers.Expenses;

namespace Invoices.API
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

            var dbConnection = Configuration.GetConnectionString("InvoicesDbConnection");
            var dbName = Configuration.GetConnectionString("InvoicesDbName");
            services.AddSingleton<IDocumentStore>(x => DocumentStoreHolder.GetStore(dbConnection, dbName));
            //services.AddTransient<IDocumentSession>(x => x.GetService<IDocumentStore>().OpenSession());
            services.AddScoped<IAsyncDocumentSession>(x => x.GetService<IDocumentStore>().OpenAsyncSession());

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoices Microservice", Version = "v1" });
            });

            services.AddControllers(x => x.UseCentralRoutePrefix(new RouteAttribute("api/")));
            
            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipel1ine.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoices Microservice V1");
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

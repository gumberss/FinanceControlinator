using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PiggyBanks.API.Commons;
using PiggyBanks.Data.Contexts;
using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Handler.Configurations;

namespace PiggyBanks.API
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

            services.AddDbContext<IPiggyBankDbContext, PiggyBankDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PiggyBanksDbConnection"));
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "PiggyBanks Microservice", Version = "v1" });
            });

            services.AddControllers(x => x.UseCentralRoutePrefix(new RouteAttribute("api/")));

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "PiggyBanks Microservice V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<TransactionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

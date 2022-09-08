using FinanceControlinator.Common.Contexts;
using FinanceControlinator.Common.CustomLogs;
using Invoices.API.Commons;
using Invoices.Data.Contexts;
using Invoices.Handler.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var rabbitMqValues = new RabbitMqValues()
{
    Host = builder.Configuration.GetSection("RabbitMq:Host").Value,
    Username = builder.Configuration.GetSection("RabbitMq:Username").Value,
    Password = builder.Configuration.GetSection("RabbitMq:Password").Value,
};

builder.Services.ConfigureMassTransit(rabbitMqValues);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

var dbConnection = builder.Configuration.GetConnectionString("InvoicesDbConnection");
var dbName = builder.Configuration.GetConnectionString("InvoicesDbName");
builder.Services.AddSingleton<IDocumentStore>(x => DocumentStoreHolder.GetStore(dbConnection, dbName));
builder.Services.AddScoped<IContext>(x => new DocumentoStoreContext(x.GetService<IDocumentStore>()));
builder.Services.AddScoped<IAsyncDocumentSession>(x => ((DocumentoStoreContext)x.GetService<IContext>()).Context());

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoices Microservice", Version = "v1" });
});

builder.Services.AddControllers(x => x.UseCentralRoutePrefix(new RouteAttribute("api/")));

builder.Services.RegisterServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddProvider(new CustomLogProvider(new CustomLogConfig()));

var app = builder
    .Build()
    .EnsureDatabaseExists();


if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoices Microservice V1");
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
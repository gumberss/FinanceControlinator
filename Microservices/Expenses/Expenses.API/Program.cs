using Expenses.API.Commons;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Handler.Configurations;
using FinanceControlinator.Common.CustomLogs;
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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var rabbitMqValues = new RabbitMqValues
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

    builder.Services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ExpensesDbConnection"));
    });



}

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

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Expenses Microservice", Version = "v1" });
});

builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services.AddControllers(x => x.UseCentralRoutePrefix(new RouteAttribute("api/")));

builder.Services.RegisterServices();

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddProvider(new CustomLogProvider(new CustomLogConfig()));


var app = builder
    .Build();
if (!app.Environment.IsDevelopment())
{
    app = app.Migrate().Result;
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Expenses Microservice V1");
    });
}

app.Run();

public partial class Program { }
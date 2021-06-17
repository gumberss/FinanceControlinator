using Expenses.Application.AppServices;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.API.Commons
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseAppService, ExpenseAppService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            
        }
    }
}

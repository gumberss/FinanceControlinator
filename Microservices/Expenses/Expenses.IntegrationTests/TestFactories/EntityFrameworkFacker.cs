using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Expenses.IntegrationTests.TestFactories
{
    internal class EntityFrameworkFacker : IFakeConfig<IExpenseDbContext>
    {
        private IExpenseDbContext _db;

        public Task Configure(IServiceCollection services)
        {
            services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            return Task.CompletedTask;
        }

        public IExpenseDbContext Get(IServiceProvider provider)
        {
            _db = provider.GetRequiredService<IExpenseDbContext>();
            ((ExpenseDbContext)_db).Database.EnsureCreated();

            return _db;
        }
    }
}

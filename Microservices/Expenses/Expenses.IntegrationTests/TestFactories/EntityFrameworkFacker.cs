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

        String dbName = Guid.NewGuid().ToString();

        public Task Configure(IServiceCollection services)
        {
            services.AddDbContext<IExpenseDbContext, ExpenseDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
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

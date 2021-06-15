using Expenses.Domain.Models;
using FinanceControlinator.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Contexts
{
    public class ExpenseDbContext : DbContext, IContext
    {
        
        public ExpenseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseItem> Items { get; set; }

        public void Commit() => this.SaveChanges();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("expenses");

            base.OnModelCreating(modelBuilder);

            //var typesToMapping = (from x in Assembly.GetExecutingAssembly().GetTypes()
            //                      where x.IsClass && typeof(IMapping).IsAssignableFrom(x)
            //                      select x).ToList();
            //// Varrendo todos os tipos que são mapeamento 
            //// Com ajuda do Reflection criamos as instancias 
            //// e adicionamos no Entity Framework
            //foreach (var mapping in typesToMapping)
            //{
            //    dynamic mappingClass = Activator.CreateInstance(mapping);
            //    modelBuilder.Configurations.Add(mappingClass);
            //}
        }
    }
}

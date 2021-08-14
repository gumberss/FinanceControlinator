using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Contexts;
using FinanceControlinator.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Data.Contexts
{
    public class ExpenseDbContext : DbContext, IExpenseDbContext
    {

        public ExpenseDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

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

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (entity is IEntity<Guid> theEntity)
            {
                theEntity.CreatedDate = DateTime.Now;
            }

            return base.Add(entity);
        }

        public override EntityEntry Update(object entity)
        {
            if (entity is IEntity<Guid> theEntity)
            {
                theEntity.UpdatedDate = DateTime.Now;
            }

            return base.Update(entity);
        }

        public async Task<int> Commit()
        {
            foreach (var item in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
            {
                if (item.State == EntityState.Added)
                {
                    item.Property("CreatedDate").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Modified)
                {
                    item.Property("CreatedDate").IsModified = false;
                }
            }

            foreach (var item in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("UpdateDate") != null))
            {
                if (item.State == EntityState.Modified)
                {
                    item.Property("UpdateDate").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}

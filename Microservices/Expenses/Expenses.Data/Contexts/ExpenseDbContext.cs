﻿using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using FinanceControlinator.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
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

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
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
            return await base.SaveChangesAsync();
        }
    }
}

using PiggyBanks.Data.Interfaces.Contexts;
using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Contexts;
using FinanceControlinator.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBanks.Data.Contexts
{
    public class PiggyBankDbContext : DbContext, IPiggyBankDbContext
    {
        
        public PiggyBankDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<PiggyBank> PiggyBanks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("piggyBanks");

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
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if(entity is IEntity<Guid> theEntity)
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
            foreach (var item in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("InsertDate") != null))
            {
                if (item.State == EntityState.Added)
                {
                    item.Property("InsertDate").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Modified)
                {
                    item.Property("InsertDate").IsModified = false;
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

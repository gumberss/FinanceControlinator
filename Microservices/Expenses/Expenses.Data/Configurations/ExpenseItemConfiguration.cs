using Expenses.Domain.Models.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Data.Configurations
{
    public class ExpenseItemConfiguration : IEntityTypeConfiguration<ExpenseItem>
    {
        public void Configure(EntityTypeBuilder<ExpenseItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(250);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .HasOne(x => x.Expense)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ExpenseId);

            builder
                .Property(x => x.CreatedDate)
                .IsRequired()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder
                .Property(x => x.UpdatedDate);
        }
    }
}

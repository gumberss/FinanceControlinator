using Expenses.Domain.Models.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.Data.Configurations
{
    class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.PurchaseDate)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(250);

            builder
                .Property(x => x.InstallmentsCount)
                .IsRequired();


            builder
                .Property(x => x.Location)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Observation)
                .HasMaxLength(500);

            builder
                .Property(x => x.Type);

            builder
                .Property(x => x.UserId)
                .IsRequired();

            builder
                .Property(x => x.CreatedDate)
                .IsRequired()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder
                .Property(x => x.UpdatedDate);

            builder
               .HasIndex(x => x.UserId)
               .HasDatabaseName("IX_Expense_UserId")
               .HasFillFactor(80);

            builder
              .HasIndex(x => x.PurchaseDate)
              .HasDatabaseName("IX_Expense_Pagination")
              .HasFillFactor(80);
        }
    }
}

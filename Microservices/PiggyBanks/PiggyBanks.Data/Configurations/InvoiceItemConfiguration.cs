using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Data.Configurations
{
    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.CreatedDate)
                .IsRequired();

            builder
                .Property(x => x.UpdatedDate);

            builder
                .Property(x => x.InstallmentCost);

            builder
                .Property(x => x.ExpenseId);

            builder
                .HasOne(x => x.Invoice)
                .WithMany(x=> x.Items)
                .HasForeignKey(x=> x.InvoiceId);
        }
    }
}
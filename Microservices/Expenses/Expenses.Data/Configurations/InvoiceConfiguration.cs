using Expenses.Domain.Models.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.CreatedDate)
                .IsRequired();

            builder
                .Property(x => x.UpdatedDate);

            builder
                .Property(x => x.DueDate);
        }
    }
}
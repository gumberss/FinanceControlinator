using Invoices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Configurations
{
    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
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
                .HasOne(x => x.Invoice)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.InvoiceId);

            builder
                .Property(x => x.InsertDate)
                .IsRequired();

            builder
                .Property(x => x.UpdateDate);
        }
    }
}

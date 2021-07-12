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
    class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(b => b.Id);
            builder
                .Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Date)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(250);

            builder.Property(x => x.IsRecurrent);

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
                .Property(x => x.InsertDate)
                .IsRequired();

            builder
                .Property(x => x.UpdateDate);
        }
    }
}

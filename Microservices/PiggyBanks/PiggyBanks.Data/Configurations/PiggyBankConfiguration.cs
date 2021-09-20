using PiggyBanks.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBanks.Data.Configurations
{
    class PiggyBankConfiguration : IEntityTypeConfiguration<PiggyBank>
    {
        public void Configure(EntityTypeBuilder<PiggyBank> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.GoalDate)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(500);

            builder
                .Property(x => x.GoalDate);

            builder
                .Property(x => x.GoalValue);

            builder
                .Property(x => x.SavedValue);

            builder
                .Property(x => x.StartDate);

            builder
                .Property(x => x.Type);

            builder
                .Property(x => x.CreatedDate)
                .IsRequired();

            builder
                .Property(x => x.UpdatedDate);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Data.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Source)
              .WithMany(b => b.SourceTransfers)
              .HasForeignKey(x => x.SourceId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Destination)
              .WithMany(b => b.DestinationTransfers)
              .HasForeignKey(x => x.DestinationId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(b => b.Amount);
        }
    }
}

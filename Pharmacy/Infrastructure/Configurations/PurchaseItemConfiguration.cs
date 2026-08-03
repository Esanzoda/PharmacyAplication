using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.CQRS.Purchase.Models;

namespace Pharmacy.Infrastructure.Configurations;

public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
{
    public void Configure(EntityTypeBuilder<PurchaseItem> builder)
    {
        builder.ToTable("PurchaseItems");

        builder.HasKey(x => x.Id);

        builder.HasOne<Purchase>()
            .WithMany(x => x.PurchaseItems)
            .HasForeignKey(x => x.PurchaseId);

        builder.Property(x => x.Barcode)
            .IsRequired()
            .HasMaxLength(100);
    }
}
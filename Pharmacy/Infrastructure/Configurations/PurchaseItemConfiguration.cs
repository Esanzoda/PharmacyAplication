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

        builder.Property(x => x.Barcode)
            .IsRequired()
            .HasMaxLength(100);


        builder.HasOne(x => x.PurchaseEntity)
            .WithMany(x => x.PurchaseItems)
            .HasForeignKey(x => x.PurchaseEntityId);
    }
}
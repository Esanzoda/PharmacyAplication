using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Expired;

namespace Pharmacy.Infrastructure.Configurations;

public class ExpiryDateItemsConfiguration : IEntityTypeConfiguration<ExpiredItemsEntity>
{
    public void Configure(EntityTypeBuilder<ExpiredItemsEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalSalePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPurchasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.ExpiredProductsEntity)
            .WithMany(x => x.ExpiryDateItemsList)
            .HasForeignKey(x => x.ExpiryDateEntityId);
    }
}
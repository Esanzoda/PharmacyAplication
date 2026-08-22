using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.CQRS.ExpiredProducts.Models;

namespace Pharmacy.Infrastructure.Configurations;

public class ExpiryDateConfiguration : IEntityTypeConfiguration<ExpiredProductsEntity>
{
    public void Configure(EntityTypeBuilder<ExpiredProductsEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalSalePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPurchasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}
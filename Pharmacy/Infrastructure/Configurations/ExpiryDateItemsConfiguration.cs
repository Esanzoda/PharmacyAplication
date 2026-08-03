using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class ExpiryDateItemsConfiguration : IEntityTypeConfiguration<ExpiryDateItems>
{
    public void Configure(EntityTypeBuilder<ExpiryDateItems> builder)
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Barcode)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        builder.Property(x => x.SalePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Stock)
            .IsRequired();


        builder.HasOne(x => x.CategoryEntity)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryEntityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
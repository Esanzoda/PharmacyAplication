using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Infrastructure.Configurations;

public class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
{
    public void Configure(EntityTypeBuilder<ProductBatch> builder)
    {
        builder.ToTable("ProductBatches");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.HasOne(x => x.ProductEntity)
            .WithMany(x => x.ProductBatches)
            .HasForeignKey(x => x.ProductEntityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
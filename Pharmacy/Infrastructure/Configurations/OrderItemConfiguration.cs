using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Order;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.HasOne<ProductEntity>()
            .WithMany()
            .HasForeignKey(x => x.ProductEntityId);
    }
}
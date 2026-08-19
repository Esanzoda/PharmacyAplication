using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.CQRS.Cart.Models;

namespace Pharmacy.CQRS.Cart.Configuration;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(x => x.Id);

        builder.HasOne<Models.CartEntity>()
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.CustomerEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.SalePrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}
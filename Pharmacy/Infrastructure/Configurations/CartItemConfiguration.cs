using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasOne<Cart>()
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}
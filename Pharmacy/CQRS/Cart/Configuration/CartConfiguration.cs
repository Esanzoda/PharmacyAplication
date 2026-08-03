using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharmacy.CQRS.Cart.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<Models.Cart>
{
    public void Configure(EntityTypeBuilder<Models.Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");
    }
}
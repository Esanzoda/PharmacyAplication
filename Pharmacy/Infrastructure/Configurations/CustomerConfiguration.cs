using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Cart;
using Pharmacy.Domain.Models.Customer;
using Pharmacy.Domain.Models.Order;

namespace Pharmacy.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.HasOne(x => x.CartEntity)
            .WithOne(x => x.CustomerEntity)
            .HasForeignKey<CartEntity>(x => x.CustomerEntityId);

        builder.HasMany<OrderEntity>()
            .WithOne(x => x.CustomerEntity)
            .HasForeignKey(x => x.CustomerEntityId);
    }
}
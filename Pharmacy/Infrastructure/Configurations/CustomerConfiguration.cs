using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Order.Models;

namespace Pharmacy.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(150);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500);

        builder.HasOne(x => x.CartEntity)
            .WithOne(x => x.CustomerEntity)
            .HasForeignKey<CartEntity>(x => x.CustomerEntityId);

        builder.HasMany<OrderEntity>()
            .WithOne(x => x.CustomerEntity)
            .HasForeignKey(x => x.CustomerEntityId);
    }
}
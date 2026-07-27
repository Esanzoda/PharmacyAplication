using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();
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

        builder.HasOne(x => x.Cart)
            .WithOne(x => x.Customer)
            .HasForeignKey<Cart>(x => x.CustomerId);

        builder.HasMany<Order>()
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);
    }
}
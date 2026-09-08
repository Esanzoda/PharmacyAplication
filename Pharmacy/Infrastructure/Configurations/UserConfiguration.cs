using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Name)
            .HasMaxLength(80);

        builder.Property(x => x.Email)
            .HasMaxLength(50);

        builder.Property(x => x.Address)
            .HasMaxLength(200);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(200);
        
    }
}
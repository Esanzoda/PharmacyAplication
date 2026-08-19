using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharmacy.Infrastructure.Configurations;

public class CompanyEmployee : IEntityTypeConfiguration<Models.Domain.CompanyEmployee>
{
    public void Configure(EntityTypeBuilder<Models.Domain.CompanyEmployee> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Name)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(50);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharmacy.Infrastructure.Configurations;

public class CompanyEmployeeConfiguration : IEntityTypeConfiguration<Models.Domain.CompanyEmployee>
{
    public void Configure(EntityTypeBuilder<Models.Domain.CompanyEmployee> builder)
    {
    }
}
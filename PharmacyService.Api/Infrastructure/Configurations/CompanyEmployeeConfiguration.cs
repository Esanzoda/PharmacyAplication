using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Api.Infrastructure.Configurations;

public class CompanyEmployeeConfiguration : IEntityTypeConfiguration<CompanyEmployee>
{
    public void Configure(EntityTypeBuilder<CompanyEmployee> builder)
    {
    }
}
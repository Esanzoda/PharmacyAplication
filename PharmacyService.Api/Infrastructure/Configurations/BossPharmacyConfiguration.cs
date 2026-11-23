using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Api.Infrastructure.Configurations;

public class BossPharmacyConfiguration : IEntityTypeConfiguration<BossPharmacy>
{
    public void Configure(EntityTypeBuilder<BossPharmacy> builder)
    {
    }
}
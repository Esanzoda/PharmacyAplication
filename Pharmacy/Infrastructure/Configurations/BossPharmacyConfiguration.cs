using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class BossPharmacyConfiguration : IEntityTypeConfiguration<BossPharmacy>
{
    public void Configure(EntityTypeBuilder<BossPharmacy> builder)
    {
    }
}
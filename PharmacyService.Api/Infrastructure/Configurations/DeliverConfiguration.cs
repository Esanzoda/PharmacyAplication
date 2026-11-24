using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Api.Infrastructure.Configurations;

public class DeliverConfiguration : IEntityTypeConfiguration<DeliverEntity>
{
    public void Configure(EntityTypeBuilder<DeliverEntity> builder)
    {
    }
}
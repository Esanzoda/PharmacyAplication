using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.Infrastructure.Configurations;

public class DeliverConfiguration : IEntityTypeConfiguration<DeliverEntity>
{
    public void Configure(EntityTypeBuilder<DeliverEntity> builder)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.CQRS.Deliver.Models;

namespace Pharmacy.Infrastructure.Configurations;

public class DeliverConfiguration : IEntityTypeConfiguration<DeliverEntity>
{
    public void Configure(EntityTypeBuilder<DeliverEntity> builder)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Domain.Models.PurchaseEntity;

namespace Pharmacy.Infrastructure.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchases");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.EmployeeEntity)
            .WithMany()
            .HasForeignKey(x => x.EmployeeEntityId);
    }
}
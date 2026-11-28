using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Domain.Entities;

namespace PharmacyService.Api.Application;

public interface IApplicationDbContext
{
    DbSet<CartEntity> Carts { get; }
    DbSet<CartItemEntity> CartItems { get; }
    DbSet<CustomerEntity> Customers { get; }
    DbSet<EmployeeEntity> Employees { get; }
    DbSet<OrderEntity> Orders { get; }
    DbSet<OrderItemEntity> OrderItems { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseItem> PurchaseItems { get; }
    DbSet<DeliverEntity> Delivers { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<PharmacyEntity> Pharmacies { get; }
    DbSet<User> Users { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<OrderItemBatch> OrderItemBatches { get; }
    public DbSet<BossPharmacy> BossPharmacies { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application;
using PharmacyService.Api.Infrastructure.Configurations;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Api.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<DeliverEntity> Delivers { get; set; }
    public DbSet<ExpiredProductsEntity> ExpireDateProducts { get; set; }
    public DbSet<ExpiredItemsEntity> ExpireDateItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PharmacyEntity> Pharmacies { get; set; }
    public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<OrderItemBatch> OrderItemBatches { get; set; }
    public DbSet<BossPharmacy> BossPharmacies { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartConfiguration).Assembly);
    }
}
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Cart;
using Pharmacy.Domain.Models.Category;
using Pharmacy.Domain.Models.Customer;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Domain.Models.Employee;
using Pharmacy.Domain.Models.Expired;
using Pharmacy.Domain.Models.Order;
using Pharmacy.Domain.Models.Pharmacy;
using Pharmacy.Domain.Models.Product;
using Pharmacy.Domain.Models.PurchaseEntity;
using Pharmacy.Infrastructure.Configurations;
using Pharmacy.Interfaces;

namespace Pharmacy.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<DeliverEntity> Delivers { get; set; }
    public DbSet<ExpiredProductsEntity> ExpireDateProducts { get; set; }
    public DbSet<ExpiredItemsEntity> ExpireDateItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PharmacyEntity> Pharmacies { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
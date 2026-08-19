using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Category.Models;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.ExpiredProducts.Models;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.Infrastructure.Configurations;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using CompanyEmployee = Pharmacy.Models.Domain.CompanyEmployee;

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
    public DbSet<ExpiryDateEntity> ExpireDateProducts { get; set; }
    public DbSet<ExpiryDateItemsEntity> ExpireDateItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<CQRS.Pharmacy.Models.PharmacyEntity> Pharmacies { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<CompanyEmployee> CompanyEmployees { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
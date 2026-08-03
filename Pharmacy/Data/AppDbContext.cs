using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Category.Models;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.Infrastructure.Configurations;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;

namespace Pharmacy.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Deliver> Delivers { get; set; }
    public DbSet<ExpiryDate> ExpireDateProducts { get; set; }
    public DbSet<ExpiryDateItems> ExpireDateItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<CQRS.Pharmacy.Models.Pharmacy> Pharmacies { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
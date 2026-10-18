using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category;
using Pharmacy.Domain.Models.Expired;
using Pharmacy.Domain.Models.Product;
using ProductService.Infrastructure.Configuration;
using ProductService.Interfaces;

namespace ProductService.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options):
    DbContext(options),IProductDbContext
{
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ExpiredProductsEntity> ExpireDateProducts { get; set; }
    public DbSet<ExpiredItemsEntity> ExpireDateItems { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }

}
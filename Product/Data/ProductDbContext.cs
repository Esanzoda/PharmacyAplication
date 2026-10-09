using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category;
using Pharmacy.Domain.Models.Product;
using Product.Infrastructure.Configuration;
using Product.Interfaces;

namespace Product.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options):
    DbContext(options),IProductDbContext
{
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }

}
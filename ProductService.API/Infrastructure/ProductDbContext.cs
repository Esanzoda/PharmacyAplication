using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application;
using ProductService.Api.Domain.Entities;
using ProductService.Api.Infrastructure.Configuration;

namespace ProductService.Api.Infrastructure;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) :
    DbContext(options), IProductDbContext
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
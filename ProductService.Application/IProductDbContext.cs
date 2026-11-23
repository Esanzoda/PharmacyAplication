using Microsoft.EntityFrameworkCore;
using ProductServiceDomain.Entities;

namespace ProductService.Application;

public interface IProductDbContext
{
    DbSet<CategoryEntity> Categories { get; }
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductBatch> ProductBatches { get; set; }
    DbSet<ExpiredProductsEntity> ExpireDateProducts { get; }
    DbSet<ExpiredItemsEntity> ExpireDateItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
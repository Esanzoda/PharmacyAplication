using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category;
using Pharmacy.Domain.Models.Expired;
using Pharmacy.Domain.Models.Product;

namespace ProductService.Interfaces;

public interface IProductDbContext
{
    DbSet<CategoryEntity> Categories { get; }
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductBatch> ProductBatches { get; set; }
    DbSet<ExpiredProductsEntity> ExpireDateProducts { get; }
    DbSet<ExpiredItemsEntity> ExpireDateItems { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Category;
using Pharmacy.Domain.Models.Product;

namespace Product.Interfaces;

public interface IProductDbContext
{
    DbSet<CategoryEntity> Categories { get; }
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductBatch> ProductBatches { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
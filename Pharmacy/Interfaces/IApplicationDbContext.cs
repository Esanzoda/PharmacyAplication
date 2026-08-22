using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Category.Models;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.ExpiredProducts.Models;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Pharmacy.Models;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.Models.Domain;

namespace Pharmacy.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CartEntity> Carts { get; }
    DbSet<CartItemEntity> CartItems { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CustomerEntity> Customers { get; }
    DbSet<EmployeeEntity> Employees { get; }
    DbSet<OrderEntity> Orders { get; }
    DbSet<OrderItemEntity> OrderItems { get; }
    DbSet<ProductEntity> Products { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseItem> PurchaseItems { get; }
    DbSet<DeliverEntity> Delivers { get; }
    DbSet<ExpiredProductsEntity> ExpireDateProducts { get; }
    DbSet<ExpiredItemsEntity> ExpireDateItems { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<PharmacyEntity> Pharmacies { get; }
    DbSet<ProductBatch> ProductBatches { get; set; }
    DbSet<CompanyEmployee> CompanyEmployees { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
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
    DbSet<User> Users { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
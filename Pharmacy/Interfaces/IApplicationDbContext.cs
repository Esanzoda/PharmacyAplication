using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Category.Models;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.Models.Domain;

namespace Pharmacy.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Product> Products { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseItem> PurchaseItems { get; }
    DbSet<Deliver> Delivers { get; }
    DbSet<ExpiryDate> ExpireDateProducts { get; }
    DbSet<ExpiryDateItems> ExpireDateItems { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<CQRS.Pharmacy.Models.Pharmacy> Pharmacies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
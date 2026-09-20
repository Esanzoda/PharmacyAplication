using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IOrderEndpoint
{
    [Post("/api/Order/Create")]
     Task<OrderResponseForCustomer> Create(
      long customerId,
        [Body] CreateOrderRequest request,
        double? newCustomerLat,
        double? newCustomerLong,
        string? newCustomerAddress);

    [Post("/api/Order/CreateFromCart")]
     Task<OrderResponseForCustomer> CreateFromCart(
      long customerId,
        OrderType orderType,
        double? newCustomerLatitude,
        double? newCustomerLongitude,
        string? newCustomerAddress);

    [Put("/api/Order/CancelOrder")]
     Task<OrderResponseForCustomer> CancelOrder(
      long customerId,
      long orderId);

     [Get("/api/Order/GetInfo")]
     Task<OrderResponseForCustomer> GetInfo(
      long customerId,
      long id);

    [Get("/api/Order/GetAll")]
     Task<List<OrderResponseForCustomer>> GetAll(
      long customerId,
      int pageNumber, 
      int pageSize);

     [Get("/api/Order/GetByStatus")]
     Task<List<OrderResponseForCustomer>> GetByStatus(
      long customerId,
      OrderStatus status, 
      int pageNumber,
      int pageSize);

     [Delete("/api/Order/RemoveItem")]
     Task<OrderResponseForCustomer> RemoveItem(
      long customerId,
      long orderId,
      long productId);
}
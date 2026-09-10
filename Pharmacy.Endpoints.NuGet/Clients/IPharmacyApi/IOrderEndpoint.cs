using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IOrderEndpoint
{
    [Post("/api/Order/Create")]
     Task<OrderResponseForCustomer> Create(
        [Body] CreateOrderRequest request,
        double? newCustomerLat,
        double? newCustomerLong,
        string? newCustomerAddress);

    [Post("/api/Order/CreateFromCart")]
     Task<OrderResponseForCustomer> CreateFromCart(
        OrderType orderType,
        double? newCustomerLatitude,
        double? newCustomerLongitude,
        string? newCustomerAddress);

    [Put("/api/Order/CancelOrder")]
     Task<OrderResponseForCustomer> CancelOrder(
      long orderId);

     [Get("/api/Order/GetInfo")]
     Task<OrderResponseForCustomer> GetInfo(
      long id);

    [Get("/api/Order/GetAllByPagination")]
     Task<List<OrderResponseForCustomer>> GetAllByPagination(
      int pageNumber, 
      int pageSize);

     [Get("/api/Order/GetByStatusAsync")]
     Task<List<OrderResponseForCustomer>> GetByStatusAsync(
      OrderStatus status, 
      int pageNumber,
      int pageSize);

     [Delete("/api/Order/RemoveItem")]
     Task<OrderResponseForCustomer> RemoveItem(
      long orderId,
      long productId);
}
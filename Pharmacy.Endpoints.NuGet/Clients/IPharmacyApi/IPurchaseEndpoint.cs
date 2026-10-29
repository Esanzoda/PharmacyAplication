using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IPurchaseEndpoint
{
    [Post("/api/Purchase/Add")]
    Task<PurchaseResponse> Add(
        long employeeId,
        [Body] PurchaseRequest request,
        long pharmacyId);

    [Get("/api/Purchase/GetById")]
    Task<PurchaseResponse> GetById(
        long id,
        long pharmacyId);

    [Get("/api/Purchase/GetAll")]
    Task<List<PurchaseResponse>> GetAll(
        long pharmacyId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/Purchase/GetPurchasesBySupplierIdForSpecificDate")]
    Task<List<PurchaseResponse>> GetPurchasesBySupplierIdForSpecificDate(
        long supplierId,
        long pharmacyId,
        [Query] DateOnly dateOnly,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/Purchase/GetPurchaseItemByPurchaseId")]
    Task<List<PurchaseItemResponse>> GetPurchaseItemByPurchaseId(
        long purchaseId,
        long pharmacyId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/Purchase/GetPurchasesByDate")]
    Task<List<PurchaseResponse>> GetPurchasesByDate(
        long pharmacyId,
        [Query] DateOnly dateOnly,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/Purchase/GetPurchasesBySupplierId")]
    Task<List<PurchaseResponse>> GetPurchasesBySupplierId(
        long supplierId,
        long pharmacyId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/Purchase/GetPurchaseByEmployeeId")]
    Task<List<PurchaseResponse>> GetPurchaseByEmployeeId(
        long pharmacyId,
        long employeeId,
        [Query] PaginationRequest paginationRequest);

    [Delete("/api/Purchase/DeleteById")]
    Task<string> DeleteById(
        long employeeId,
        long id,
        long pharmacyId);

    [Post("/api/Purchase/AddItem")]
    Task<PurchaseResponse> AddItem(
        long pharmacyId,
        long purchaseId,
        PurchaseItemRequest purchaseItemRequest);


    [Delete("/api/Purchase/RemoveItem")]
    Task<PurchaseResponse> RemoveItem(
        long employeeId,
        long purchaseId,
        long pharmacyId,
        long purchaseItemId);
}
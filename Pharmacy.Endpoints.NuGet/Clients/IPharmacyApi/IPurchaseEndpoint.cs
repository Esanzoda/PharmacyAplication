using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IPurchaseEndpoint
{
   [Post("api/Purchase/Add")]
    Task<PurchaseResponse> Add(
     [Body] PurchaseRequest request,
     long pharmacyId);
    
   [Get("api/Purchase/GetById")]
  Task<PurchaseResponse> GetById(
   long id, 
   long pharmacyId);
  
   [Get("api/Purchase/GetAll")]
    Task<List<PurchaseResponse>> GetAll(
     long pharmacyId, 
     int pageNumber, 
     int pageSize);
    
   [Delete("api/Purchase/DeleteById")]
   Task DeleteById(
    long id, 
    long pharmacyId);
   
   [Post("api/Purchase/AddItem")]
  Task<PurchaseItemResponse> AddItem(
   long pharmacyId, 
   long purchaseId,
      PurchaseItemRequest purchaseItemRequest);
  
   [Delete("api/Purchase/RemoveItem")]
   Task<PurchaseItemResponse> RemoveItem(
    long purchaseId, 
    long pharmacyId,
      long purchaseItemId);
}
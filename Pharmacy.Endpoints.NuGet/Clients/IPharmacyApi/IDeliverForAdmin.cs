using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IDeliverForAdmin
{
    [Post("/api/DeliverForAdmin/")]
    Task<DeliverResponse> Add(
     [Body] DeliverRequest request);


    [Put("/api/DeliverForAdmin/Update")]
  Task<DeliverResponse> Update(
   long id, 
   [Body] UpdateDeliverRequest request);


    [Get("/api/DeliverForAdmin/GetById")]
    Task<DeliverResponse> GetById(
     long id);


    [Get("/api/DeliverForAdmin/GetByEmail")]
   Task<List<DeliverResponse>> GetByEmail(
    string email);


    [Get("/api/DeliverForAdmin/GetAll")]
     Task<List<DeliverResponse>> GetAll(
      int pageNumber, 
      int pageSize);


    [Delete("/api/DeliverForAdmin/DeleteById")]
     Task DeleteById(
      long id);

}
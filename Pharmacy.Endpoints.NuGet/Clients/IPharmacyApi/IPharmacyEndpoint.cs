using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IPharmacyEndpoint
{
    [Patch("api/Pharmacy/UpdateName")]
   Task<PharmacyResponse> UpdateName(
        [Body] string newName, 
        long pharmacyId);

    [Patch("api/Pharmacy/UpdateAddress")]
    Task<PharmacyResponse> UpdateAddress(
        [Body] string nawAddress, long pharmacyId);

    [Patch("api/Pharmacy/UpdateEmail")]
     Task<PharmacyResponse> UpdateEmail(
        [Body] string newEmail, long pharmacyId);

    [Patch("api/Pharmacy/UpdatePhoneNumber")]
    Task<PharmacyResponse> UpdatePhoneNumber(
        [Body] string nawNumber, 
        long pharmacyId);

    [Get("api/Pharmacy/GetExpiryProducts")]
    Task<List<ExpiredResponse>> GetExpiryProducts(
        long pharmacyId, 
        int pageNumber,
        int pageSize);
}